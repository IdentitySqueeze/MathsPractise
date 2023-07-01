using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fangorn;
using Polish;
using MathUtils;


namespace DustBlowerClient {
    public static class _ {
        // -- optional VB'isms --
        public static bool But(this bool source, bool exp) { return exp && source; }
        public static bool ButNot(this bool source, bool exp) { return !exp && source; }
        public static bool Nor(this bool source, bool exp) { return !exp && source; } //Neither(). only
        public static bool And(this bool source, bool exp) { return exp && source; }
        public static bool Or(this bool source, bool exp) { return exp || source; }
    }

    public class QABubble { // I have my own font (powers)... so do the IQuestions (decoupled)... so does the Form
        #region variables
        public long id { get; set; }
        public IQuestion iquestion { get; set; }
        public string Hints { get; set; } // ...
        public bool _answered { get; set; }
        public bool Deleted { get; set; }
        public void Delete() => Deleted = true;
        public bool Selected { get; set; }

        // -- Drawing stuff --
        public Font font { get; set; } = new Font("Courier", 10);
        public Font hintsFont { get; set; } = new Font("Arial", 15);
        public Point position { get; set; }
        private int pad { get; set; } = 5;

        public int queWidth { get; set; }
        public int queHeight { get; set; }

        public Rectangle queRect { get; set; }
        private Rectangle askRect { get; set; }
        private Rectangle ansRect { get; set; }
        private Rectangle nullRect { get; set; } = new Rectangle(0, 0, 500, 500);

        private Pen quePen { get; set; } = new Pen(Color.Black, 2);
        private Pen askPen { get; set; } = new Pen(Color.Blue, 2);
        private Pen ansPen { get; set; } = new Pen(Color.Green, 2);
        private Pen redPen { get; set; } = new Pen(Color.Red, 2);
        private Pen blackPen { get; set; } = new Pen(Color.Black, 2);
        private Pen divisionPen { get; set; } = new Pen(Color.Black, 2);

        // -- Get variables out of the Draw( ) --
        public int letterHeight { get; set; } = 19;
        public List<wfPossibleAnswer> wfPossibleAnswers { get; set; } = new List<wfPossibleAnswer>();
        public DrawArgsRtns drawRtn { get; set; }
        public MeasureArgsRtns measureRtn { get; set; } = new MeasureArgsRtns();
        public SizeArgsRtns sizeRtn { get; set; } = new SizeArgsRtns();

        public ITreeWalker<qColumn, ConvertArgsRtns<wfNode>> convertWalker;// = new TreeWalker<qColumn, ConvertArgsRtns<wfNode>>();
        public ITreeWalker<wfNode, MeasureArgsRtns> measureWalker;// = new TreeWalker<wfNode, MeasureArgsRtns>();
        public ITreeWalker<wfNode, SizeArgsRtns> layoutWalker;// = new TreeWalker<wfNode, SizeArgsRtns>();
        public ITreeWalker<wfNode, DrawArgsRtns> drawWalker;// { get; set; } = new TreeWalker<wfNode, DrawArgsRtns>();

        public Treebeard<qColumn, wfNode> original = new Treebeard<qColumn, wfNode>();
        #endregion
        public QABubble(long Id) { id = Id; }
        public QABubble(qParameters qparams, IQuestion iQuestion, int height, long Id) {
            id = Id;
            iquestion = iQuestion;
            Hints = iquestion.Hints;
            letterHeight = (int)font.Size+5;
            setUp();
        }
        private void setUp() {

            #region convert walker
            var convertFactory = new Treebeard<qColumn, wfNode>.ConvertFactory<qColumn, ConvertArgsRtns<wfNode>>();
            var convertRtn = new ConvertArgsRtns<wfNode>();
            convertWalker =(ITreeWalker<qColumn, ConvertArgsRtns<wfNode>>)convertFactory.GetWalker(iquestion.genus);

            // wires up all the tree ops
            convertFactory.GetWalkerOps(iquestion.genus, convertWalker);

            // run the converter
            foreach (possibleAnswer possAnswer in iquestion.possibleAnswers) {
                convertRtn = new ConvertArgsRtns<wfNode>();
                convertRtn.currentNode = new wfNode(); // dummy root
                convertRtn = convertWalker.Traverse(possAnswer.answer, convertRtn);
                wfPossibleAnswer wfPossAnswer = new wfPossibleAnswer();
                wfPossAnswer.answer.AddRange(convertRtn.currentNode.columns);
                wfPossAnswer.IsSequence = possAnswer.IsSequence;
                wfPossAnswer.uniformSize= possAnswer.uniformSize;
                wfPossibleAnswers.Add(wfPossAnswer);
            }
            #endregion

            #region measure walker
            var measureFactory = new Treebeard<qColumn, wfNode>.MeasureFactory<wfNode, MeasureArgsRtns>();
            measureRtn = new MeasureArgsRtns();
            measureWalker = (ITreeWalker<wfNode, MeasureArgsRtns>)measureFactory.GetWalker(iquestion.genus);

            // wire up the tree ops
            measureFactory.GetWalkerOps(iquestion.genus, measureWalker);

            // run the measurer
            foreach (wfPossibleAnswer possAnswer in wfPossibleAnswers) {
                measureRtn = new MeasureArgsRtns();
                measureRtn.Height = letterHeight;
                measureRtn = measureWalker.Traverse(possAnswer.answer, measureRtn);
            }
            #endregion

            #region layout walker
            var layoutFactory = new Treebeard<wfNode, wfNode>.LayoutFactory<wfNode, SizeArgsRtns>();
            layoutWalker = (ITreeWalker<wfNode, SizeArgsRtns>)layoutFactory.GetWalker(iquestion.genus);
            layoutFactory.GetWalkerOps(iquestion.genus, layoutWalker);

            //var layoutWalker = new TreeWalker<wfNode, SizeArgsRtns>();
            //layoutWalker.RowOp = SizeProcessRows;
            //layoutWalker.PreColumnOp = SizePreColumnOp;
            //layoutWalker.PostColumnOp = SizePostColumnOp;

            // run layout
            foreach (wfPossibleAnswer possAnswer in wfPossibleAnswers) {
                sizeRtn = new SizeArgsRtns(); //T
                                              //his would be a problem..
                sizeRtn.Height = letterHeight;
                if (possAnswer.uniformSize) {
                    sizeRtn.uniformSize = possAnswer.uniformSize;
                    sizeRtn.uniformedWidth = possAnswer.answer.Max(ans => ans.rows.Max(ar => ar.rowLen));
                }
                sizeRtn = layoutWalker.Traverse(possAnswer.answer, sizeRtn);
            }
            queWidth = Math.Max(iquestion.askBitmap.Width, sizeRtn.Width);
            queHeight = iquestion.askBitmap.Height+sizeRtn.Height;

            #endregion

        }
        public int GetStringLength(string txt, Font font) {
            using (Graphics g = Graphics.FromImage(new Bitmap(1, 1))) {
                return GetStringLength(txt, g, font, nullRect, null, null)+10;
            }
        }

        public bool SetPosition(Point p) {
            position = p;

            #region draw walker
            var drawFactory = new Treebeard<wfNode, wfNode>.DrawFactory<wfNode, DrawArgsRtns>();
            drawWalker = (ITreeWalker<wfNode, DrawArgsRtns>)drawFactory.GetWalker(iquestion.genus);
            drawFactory.GetWalkerOps(iquestion.genus, drawWalker);

            drawRtn = new DrawArgsRtns {
                TopLeft = new Point(position.X, position.Y),
                Selected = Selected,
                Bitmap = new Bitmap(sizeRtn.Width, sizeRtn.Height),
                Width = sizeRtn.Width,    // ?
                Height = sizeRtn.Height,  // ?
            };
            #endregion

            queRect = new Rectangle(position.X - pad, position.Y - pad, queWidth + 2 * pad, iquestion.askBitmap.Height + drawRtn.Height + 10 + 3 * pad);
            askRect = new Rectangle(position.X + (queWidth - iquestion.askBitmap.Width) / 2, position.Y, iquestion.askBitmap.Width, iquestion.askBitmap.Height + 10);
            ansRect = new Rectangle(position.X + (queWidth - drawRtn.Bitmap.Width) / 2, position.Y + askRect.Height, drawRtn.Width, drawRtn.Height + 3 * pad);

            return true;
        }

        //     measureWalker creates toBlock
        //     keysWalker updates toBlock
        //     drawWalker reads toBlock

        #region talk
        // Starts with columns.7
        // When it gets to rows, tree traversal puts me here
        // .. I'm a row if I'm here..
        // All rows have columns
        //    Some might be brackets
        //        columns
        //           |-----rows
        //                   |-----columns (bracket)
        //                             |----- ... then what?
        //    Some might be root wraps/surds
        //        columns
        //           |-----rows
        //                   |-----columns (root)
        //                             |----- ... then what?
        //    Some might be leaves
        //        columns
        //           |-----rows
        //                   |-----columns
        //                             |----- .Leaf
        //                           ( |----- .Leaf )
        //                           ( |-----   ... )
        //    Some might have exponents
        //        columns
        //           |-----rows
        //                   |-----columns
        //                             |----- .Leaf
        //                                       |-----columns (exponent group)
        //                                                 |----- ... then what?
        //
        //    Some might be leaves mixed with other things
        //        columns
        //           |-----rows (n)
        //                   |-----columns (container) (leaf level start)  --- below here is a 2-row ceiling ---
        //                             |----- .Leaf (c) (integer)          ---   for leaves and exponents    ---
        //                             |----- .Leaf (c) (integer) 
        //                             |----- column (bracket) (c) (container)
        //                                      {
        //                                                |----- .Leaf (c) (integer)
        //                                                |----- .Leaf (c) (integer)
        //                                                |----- .Leaf (c) (integer)
        //                                                |----- column (bracket) (c) (nested) (container)
        //                                                         {
        //     ...  ROWS ALWAYS STACKED VERTICALLY   ...            |----- .Leaf (c) (integer)
        //     ...COLUMNS ALWAYS NESTED OR HORIZONTAL...            |----- .Leaf (c) (integer)
        //                                                          |----- .Leaf (c) (integer)
        //                                                         }
        //                                      }
        //                             |----- .Leaf
        //                                       |-----column (exponent group) (container)
        //                                                |----- .Leaf (c) (integer)
        //                                                |----- .Leaf (c) (integer)
        //                                                |----- column (exponent fraction) (container)
        //                                                           |----- rows (2)
        //                                                                    |----- .Leaf (c) (integer)
        //                                                                    |----- .Leaf (c) (integer)
        //                                                |----- .Leaf (c)
        //                             |----- .Leaf  (integer)
        //                             |----- column (fraction) (container)
        //                                        |----- rows (2)
        //                                                 |----- .Leaf (c) (integer)
        //                                                 |----- .Leaf (c) (integer)
        //                             |----- .Leaf  (integer)
        //
        //    Some might be brackets with exponents
        //        columns
        //           |-----rows
        //                   |-----columns (bracket)
        //                             |----- ... then what?

        //                             |----- .Leaf
        //                           ( |----- .Leaf )
        //                           ( |-----   ... )
        //                             |-----columns (exponent group)
        //                                       |----- ... then what?

        // --------------------------------------------------------------
        // AXIOM: columns (n)   brackets make nestable                   
        //           |-----rows (n)  int 1, fractions 2, matrices n      
        //                   |-----columns (n), no leaves before here    
        //                                      
        //
        //
        //
        // ----------------------------------------------------------- \\
        #endregion

        #region// -- Size stuff --
        public SizeArgsRtns SizeProcessRows(wfNode answerCol, SizeArgsRtns rtn) {
            foreach (var ansRow in answerCol.rows) {
                if (ansRow.nodeValue.Trim() != ",") {
                    ansRow.rowLen+=10;
                }
                if (rtn.uniformSize) {
                    ansRow.rowLen= rtn.uniformedWidth+10;
                }
            }
            //Sizing a single column/fraction
            int maxWidth = answerCol.rows.Max(r => r.rowLen);
            if (answerCol.rows.Count == 1) { //Single row
                answerCol.rows[0].rowRect = new Rectangle(rtn.Width, 0, maxWidth, letterHeight);
            } else if (answerCol.rows.Count == 2) { //Double row
                rtn.Height =Math.Max(rtn.Height, letterHeight * 2 + 10);
                answerCol.rows[0].rowRect
                = new Rectangle(rtn.Width, 0, answerCol.rows[0].rowLen, letterHeight);
                answerCol.rows[1].rowRect
                = new Rectangle(rtn.Width, letterHeight + 10, answerCol.rows[1].rowLen, letterHeight);
            }
            rtn.Width += maxWidth;
            return rtn;
        }
        public SizeArgsRtns SizePreColumnOp(wfNode ac, SizeArgsRtns rtn) {
            if (ac.colType==ColTyp.bracket)
                rtn.Width +=10;
            if (ac.colType==ColTyp.sigma) {
                int len = GetStringLength(ac.from+"__", font);
                ac.rowRect=new Rectangle(rtn.Width, 0, len, 1);
                rtn.Width+=len;
            }
            return rtn;
        }
        public SizeArgsRtns SizePostColumnOp(wfNode ac, SizeArgsRtns rtn) {
            float mid = (float)rtn.Height/2f;
            float dynY = (mid- ((float)ac.rows.Count/2f*letterHeight));// TODO: sync

            if (ac.colType==ColTyp.bracket)
                rtn.Width +=10;
            if (ac.colType==ColTyp.sigma) {
                ac.rowRect=new Rectangle(ac.rowRect.X, (int)dynY, rtn.Width, 3*letterHeight);
                rtn.Height=Math.Max(rtn.Height, 5*letterHeight);
            }

            return rtn;
        }
        #endregion
        public DrawArgsRtns DrawPostTraversal(List<wfNode> answerRows, DrawArgsRtns rtn) { //<-- QBubble Q&A bitmaps combining is in here
            // -- The question --

            rtn.g.FillRectangle(Brushes.Gainsboro, queRect); // Clear a space ..
            if (Selected) { // Select a border pen ...
                quePen = redPen;
            } else {
                quePen = blackPen;
            }
            // -- The ask ---
            rtn.g.DrawImage(iquestion.askBitmap, askRect.X, askRect.Y); //Left-aligned

            // -- The answer --
            rtn.g.DrawImage(rtn.Bitmap, ansRect.X, ansRect.Y);

            // -- Draw border --
            rtn.g.DrawRectangle(quePen, queRect);

            return rtn;
        }

        public void Draw(Graphics g, bool hints, bool answers) {
            drawRtn.g = g;
            drawRtn.ShowAnswers=answers;            

            using (Graphics gr = Graphics.FromImage(drawRtn.Bitmap)) {
                drawRtn.ansBackBuffer = gr;
                drawRtn = drawWalker.Traverse(wfPossibleAnswers[0].answer, drawRtn); // <-- Drawing (bitmap construction) is in here...
                DrawPostTraversal(wfPossibleAnswers[0].answer, drawRtn);
            }

            #region hints
            // -------------------------------
            // ... hints could be by answer, column, row or chunk
            // ... can refer to values in the row/step/col/chunk...
            // ---------
            // make hints text or image, too?
            if (hints && Selected) {
                int width = 0;
                Rectangle hRect ;
                Rectangle outRect;
                if (iquestion.hintsBitmap == null) {
                    StringFormat hFmt = new StringFormat();
                    hFmt.SetMeasurableCharacterRanges(new CharacterRange[] { new CharacterRange(0, Hints.Length) });
                    Region[] hRegions = g.MeasureCharacterRanges(Hints, hintsFont, nullRect, hFmt);
                    width = (int)hRegions.Select(r => r.GetBounds(g).Width).Sum()+50;
                    hRect = new Rectangle(55, 50, width, 40);
                    outRect = new Rectangle(50, 50, width + 10, 40);
                    g.FillRectangle(Brushes.DarkBlue, outRect);
                    g.DrawRectangle(new Pen(Color.Yellow, 1), outRect);
                    TextRenderer.DrawText(g, Hints, hintsFont, hRect, Color.Yellow);
                } else {
                    outRect = new Rectangle(50, 50, iquestion.hintsBitmap.Width+4, iquestion.hintsBitmap.Height+4);
                    g.FillRectangle(Brushes.DarkBlue, outRect);
                    g.DrawImage(iquestion.hintsBitmap, new Point(52,52));
                }
            }
            #endregion
        }

        #region // -- everything else --
        public bool Clicked(MouseEventArgs e) {
            if ((e.X > queRect.X && e.X < queRect.X + queRect.Width) &&
                (e.Y > queRect.Y && e.Y < queRect.Y + queRect.Height)) {
                Selected = true;
                return true;
            }
            Selected = false;
            return false;
        }
        public void SetAnswered() {
            _answered = Answered();
            askPen.Color = Color.Green;
            quePen.Color = Color.Green;
            ansPen.Color = Color.Green;
        }
        public bool IsAnswered() => wfPossibleAnswers.Any(ans =>
                                           ans.answer.All( wf =>
                                              wf.rows.All( ar => ar.answered)));

        //possibleAnswers < < wfNode > >
        //      |- answer   < wfNode >
        //           |- answerColumns < wfNode >
        //                    |- answerRowChunks < wfNodeElement > answered
        //
        public bool Answered() => wfPossibleAnswers.Any(ap => ap.answer.Any(ans => ans.answered));
        #endregion

        #region graphics stuff
        // -- Graphics --
        //Winforms helper functions
        public StringFormat GetStringFormat(string str) {
            StringFormat strFmt = new StringFormat();
            strFmt.SetMeasurableCharacterRanges(new CharacterRange[] { new CharacterRange(0, str.Length) });
            return strFmt;
        }
        public StringFormat GetStringFormat(CharacterRange[] crArray) {
            StringFormat strFmt = new StringFormat();
            strFmt.SetMeasurableCharacterRanges(crArray);
            return strFmt;
        }
        public Region[] GetRegionArray(string str, Font font, Rectangle rect, StringFormat strFmt, Graphics g) {
            if (strFmt is null)
                strFmt = GetStringFormat(str);
            return g.MeasureCharacterRanges(str, font, rect, strFmt);
        }
        public Region[] GetRegionArray(string str, Font font, Rectangle rect, CharacterRange[] crArray, Graphics g) {
            StringFormat strFmt = GetStringFormat(crArray);
            return g.MeasureCharacterRanges(str, font, rect, strFmt);
        }
        public int GetStringLength(string str, Graphics g, Font font, Rectangle rect, StringFormat strFmt, Region[] region) {
            if (region is null) // Get whole string
                region = GetRegionArray(str, font, rect, strFmt, g);
            return (int)region.Select(r => r.GetBounds(g).Width).Sum();
        }
        #endregion
    } // QABubble
    public class InfiniteMathTest : Form {
        public int maxQuestions { get; set; } = 1;

        #region vars
        // -- optional VB'isms --
        public static bool Neither(bool exp) { return !exp; } //.Nor() only
        public static bool Either(bool exp) { return exp; } //.Or() only
        public static bool Not(bool exp) { return !exp; }

        Bitmap Backbuffer;

        Button goBtn;
        Button stopBtn;

        Button hintsBtn;
        Button answerBtn;
        Button baseBtn;
        Button powerBtn;
        Button rootBtn;
        Button piBtn;
        Button plusOrMinusBtn;

        public int screenWidth;
        public int screenHeight;
        public int factoriesWidth;
        public int questionsWidth;
        public int controlsWidth;
        public int questionAreaWidth;
        public int questionAreaHeight;

        public Font font { get; set; } = new Font("Arial", 10);
        string DifficultySetting { get; set; } = "Basic";

        TableLayoutPanel goPanel = new TableLayoutPanel();
        TableLayoutPanel controlsPanel = new TableLayoutPanel();
        TableLayoutPanel fHousePanel = new TableLayoutPanel();
        TableLayoutPanel qHousePanel = new TableLayoutPanel();
        List<qPanel> qpanels = new List<qPanel>();

        List<string> keysIn { get; set; } = new List<string>();

        public ITreeWalker<wfNode, KeysArgsRtns> KeysWalker;
        public Treebeard<wfNode, wfNode>.KeysFactory<wfNode,KeysArgsRtns> keysFactory = new Treebeard<wfNode, wfNode>.KeysFactory<wfNode, KeysArgsRtns>();

        public Color inKeysColour { get; set; } = Color.Black;
        string keyJustPressed = string.Empty;
        Dictionary<string, bool> keyStates = KeyStates();
        Dictionary<string, bool> states = new Dictionary<string, bool>{
            {"power", false },
            {"base", false },
            {"root", false },
            {"pi", false },
        };
        public bool rooted = false;
        public bool pied = false;
        string showme = "";
        bool bShowAnswers = false;
        bool bShowHints = false;

        List<qQuestionFactory> factories { get; set; } = new List<qQuestionFactory>();
        List<QABubble> questions = new List<QABubble>();
        QABubble selectedQuestion = new QABubble(0);
        int questionsChecked { get; set; } = 0;
        bool selection = false;
        wfParametersFactory paramsFactory { get; set; } = new wfParametersFactory();
        int lastFactory { get; set; } = 0;

        public long id { get; set; }
        #endregion
        static Form InfiniteTest;
        [STAThread]
        static public void Main() {
            InfiniteTest = new InfiniteMathTest();
            Application.Run(InfiniteTest);
        }
        public InfiniteMathTest() {
            InitializeComponent();
            this.ResizeEnd += new EventHandler(Form1_CreateBackBuffer);
            this.Load += new EventHandler(Form1_CreateBackBuffer);
            this.Paint += new PaintEventHandler(Form1_Paint);
            this.KeyPress += new KeyPressEventHandler(Form1_KeyPressed);
            this.MouseClick += new MouseEventHandler(Form1_Click);
            this.KeyUp += new KeyEventHandler(Form1_KeyUp);
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
        }
        void Form1_KeyUp(object sender, KeyEventArgs e) {
            showme += "Up...";
            keyStates[e.KeyCode.ToString()] = false;
            // -- Delete --
            if ((e.KeyCode == Keys.Delete) && (selection)) {
                lock (questions) {
                    for (int i = 0; i < questions.Count; i++) {
                        if (questions[i].id == selectedQuestion.id) {
                            selection = false;
                            questions.RemoveAt(i);
                            i = questions.Count;
                        }
                    }
                    for (int i = 0; i < questions.Count; i++) {
                        questions[i].Selected = false;
                    }
                }
                states["power"] = false;
                states["base"] = false;
                states["root"] = false;
                keysIn.Clear();
                Invalidate();
                Draw();
            }
            // -- Escape --
            if (e.KeyCode == Keys.Escape) {
                keysIn.Clear();
                states["power"] = false;
                states["base"] = false;
                states["root"] = false;
                Invalidate();
                Draw();
            }
            // -- Enter --
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return) {
                keysIn.Clear();
                states["power"] = false;
                states["base"] = false;
                states["root"] = false;
                Invalidate();
            }
            // -- Backspace --
            if (e.KeyCode == Keys.Back && keysIn.Count > 0) {
                keysIn.RemoveAt(keysIn.Count - 1);
                if (keysIn.Count > 0)
                    keysIn.RemoveAt(keysIn.Count - 1);
                Invalidate();
                Draw();
                //return;
            }
        }
        void Form1_KeyDown(object sender, KeyEventArgs e) {
            keyStates[e.KeyCode.ToString()] = true;
            keyJustPressed = $@"{e.KeyCode}";

            showme += "Down..";
            //if ((Control.ModifierKeys == Keys.Control || Control.ModifierKeys == Keys.ControlKey ||
            //     Control.ModifierKeys == Keys.LControlKey || Control.ModifierKeys == Keys.RControlKey)
            //     && e.KeyCode.ToString() == "R") {
            //    //states["root"] = true;
            //    showme += "Root..";
            //    return;
            //}

            if ((Control.ModifierKeys == Keys.ShiftKey || Control.ModifierKeys == Keys.Shift ||
                 Control.ModifierKeys == Keys.RShiftKey || Control.ModifierKeys == Keys.LShiftKey)
                 && e.KeyCode.ToString() == "D6") { //^
                states["power"] = !states["power"];
                states["base"] = false;
                //states["root"] = false;
                showme += "Power..";
                return;
            }

            if ((Control.ModifierKeys == Keys.ShiftKey || Control.ModifierKeys == Keys.Shift ||
                 Control.ModifierKeys == Keys.RShiftKey || Control.ModifierKeys == Keys.LShiftKey)
                 && e.KeyCode.ToString() == "B") {
                showme += "Base..";
                states["base"] = !states["base"];
                states["power"] = false;
                return;
            }

            //if ((Control.ModifierKeys == Keys.ShiftKey || Control.ModifierKeys == Keys.Shift ||
            //      Control.ModifierKeys == Keys.RShiftKey || Control.ModifierKeys == Keys.LShiftKey)
            //      && e.KeyCode.ToString() == "P") {
            //    states["power"] = false;
            //    states["pi"] = true;
            //    showme += "Pi..";
            //    return;
            //}

            if (e.KeyCode == Keys.Control || e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey ||
               e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.Shift || e.KeyCode == Keys.RShiftKey || e.KeyCode == Keys.LShiftKey) {
            }
            // -- Escape --
            //if( e.KeyCode == Keys.Escape ) {
            //    states["power"] = false;
            //    states["base"] = false;
            //    states["root"] = false;
            //    keysIn.Clear();
            //    Invalidate( );
            //}
            // -- Enter --
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return) {
                states["power"] = false;
                states["base"] = false;
                //states["root"] = false;
                Invalidate();
            }
        }
        void Form1_KeyPressed(object sender, KeyPressEventArgs e) {
            ProcessInput(e.KeyChar.ToString());
        }
        //

        public void ProcessInput(string key) {
            keyStates[key] = true;
            lock (questions) {
                showme += "Pressed..";
                //Listen for my answers here
                if (key == "" || key == "^")
                    return;
                //if( e.KeyChar == ( char )27 ) { //Escape key
                //    inKeysColour = Color.Black;
                //    keysIn.Clear( );
                //    Invalidate( );
                //    return;
                //}

                if (states["power"]) {//&& new List<string>{"0", "1","2","3","4","5","6","7","8","9" }.Contains(key)){
                    key = GraphicsUtils.ToSuper(key);
                }

                if (states["base"]) {//&& new List<string>{"0", "1","2","3","4","5","6","7","8","9" }.Contains(key)){
                    key = GraphicsUtils.ToSub(key);
                }

                //if (states["root"])
                //    key = "√";
                //states["root"] = false;

                //if (states["pi"])
                //    key = "π";
                //states["pi"] = false;

                keysIn.Add(key);

                // -- Hints --
                if ((key == "h" || key == "H") && selection) {
                    ShowHints(); // Or..
                    return;
                }
                if (key == "A" && selection) {
                    ShowAnswers();
                    return;
                }

                string inputSoFar = String.Join("", keysIn);
                //  inputSoFar aiming for ONE answerChunk element...
                //  Compare inputSoFar against any or next (first) 
                //  answerChunk element from every questions' answer(s)...

                KeysArgsRtns rtns = new KeysArgsRtns { hit = false, keepIt = false, Exit = true, IsSequence = false };
                rtns.InputSoFar = inputSoFar;
                // var visiterator = walkerFactory.CreateWalker( question );

                //KeysWalker = new TreeWalker<wfNode, KeysArgsRtns>();

                //KeysWalker.RowOp=KeysProcessRows;
                for (int i = 0; i < questions.Count; i++) { // each question...
                    if (!selection || questions[i].id == selectedQuestion.id) {
                        foreach (wfPossibleAnswer possAnswer in questions[i].wfPossibleAnswers) { //List of correct answers
                            #region pseudo
                            // for each answerable answerChunk (sequence of terms) in (only) the next Row...
                            // A for loop ... that loops over the RowOrCols Columns...
                            //                      loops over the RowOrCols Rows...
                            //                      until it finds the first unanswered row...
                            //                          loops the answerChunk collection for that row...
                            //                          matches each element against InputSoFar...
                            //                              Yes: 'True's' the first answered element found in that Row
                            //                                     marks Row as answered if all answerChunks are True'd
                            //                          marks question as answered if all Rows are answered
                            //                      breaks/continues
                            // Yes.
                            //for( int arc=0; arc<ap.answerRowsCols.Count; i++ ){
                            //    rtns=SomethingRecurseyWithLotsOfArgmentsAndReturnVals( ap, ap.answerRowsCols[ arc ], inputSoFar );
                            //    //break check
                            //}
                            #endregion
                            // All this traversal, every time, for just one answerChunk search/update...
                            rtns.Exit = false;
                            rtns.IsSequence = possAnswer.IsSequence;
                            // Loop answer
                            //    Loop rows (2)
                            //       Loop chunks


                            // ONE keypress (passing one (or more) keys/digits)
                            KeysWalker = (ITreeWalker<wfNode, KeysArgsRtns>)keysFactory.GetWalker(questions[i].iquestion.genus);
                            keysFactory.GetWalkerOps(questions[i].iquestion.genus, KeysWalker);
                            rtns = KeysWalker.Traverse(possAnswer.answer, rtns);
                            // answers (maybe) ONE value (int/double, num, denom, row in matrix)
                            inKeysColour= rtns.inKeysColour;


                            Draw();
                            if (rtns.hit) { // chunk hit, Row updated...
                                break;//continue?
                            }
                            // if possibleAnswer doesn't hit, it will loop to the next one...
                            // ...first come, first served, if any...
                            #region unload
                        } // each possibleAnswer
                    } // selected/all...?
                    if (rtns.hit) { // something hit..  ( and updated the chunk )
                        if (questions[i].IsAnswered()) { // All Rows in one ( any ) of the possible answers answered?
                            states["power"] = false;
                            states["base"] = false;
                            object arg = i;
                            Task.Factory.StartNew((j) => {
                                System.Threading.Thread.Sleep(1500);
                                if (questions[(int)j].id == selectedQuestion.id)
                                    selection = false;
                                lock (questions)
                                    questions[(int)j].Delete();
                                Draw();
                            }, arg);
                        } // each question
                        keysIn.Clear();
                        Draw();
                        return;
                    } // chunk hit...?
                    // else  
                    //      nothing hit...
                    //      but don't return because we still (might) have other questions
                } // each question
                // if we've got here, no questions had a hit...
                rtns.hit = false;
                if (!rtns.keepIt) // keepIt refers to inputSoFar... can only keep if partially matched but didn't hit...
                    inKeysColour = Color.Red; //Wrong answer, selected ( or all ) question(s). ( or matched completely )
            } // lock( questions )
        }

        private void ShowAnswers() {
            bShowAnswers = true;
            keysIn.Clear();
            Draw();
            Task.Factory.StartNew(() => {
                Thread.Sleep(1500);
                bShowAnswers = false;
                Draw();
            });
        }

        private void ShowHints() {
            bShowHints = true;
            keysIn.Clear();
            Draw();
            Task.Factory.StartNew(() => {
                Thread.Sleep(3000);
                bShowHints = false;
                Draw();
            });
        }
        #endregion

        void Draw() {
            if (Backbuffer != null) {
                using (var g = Graphics.FromImage(Backbuffer)) {
                    //Draw all my questions here
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.Clear(Color.Black);
                    for (int i = 0; i < questions.Count; i++) {
                        if (questions[i].Deleted) {
                            //lock(questions){ //locked in caller
                            if (questions[i].Selected) {
                                selection = false;
                            }
                            questions.RemoveAt(i);
                        }
                    }
                    for (int i = 0; i < questions.Count; i++) {
                        questions[i].Draw(g, bShowHints, bShowAnswers);
                    }
                    // ----------------------------
                    //inKeys
                    Rectangle rect = new Rectangle(questionAreaWidth-140, questionAreaHeight-70, 150, 30);
                    Pen p = new Pen(Color.Black);
                    p.Width = 2;
                    g.DrawRectangle(p, rect);
                    g.FillRectangle(
                        states["power"] || states["base"] || states["root"] ? Brushes.Yellow : Brushes.White,
                        new Rectangle(rect.X + 2, rect.Y + 2, rect.Width - 4, rect.Height - 4));
                    //TextRenderer.DrawText(g, string.Join("", keysIn), this.Font,rect, inKeysColour);                        
                    g.DrawString(string.Join("", keysIn), this.Font, new SolidBrush(inKeysColour),
                        new Rectangle(rect.X + 2, rect.Y + 4, rect.Width, rect.Height));
                }
                Invalidate();
            }
        }

        #region other stuff
        public class fPanel : Panel {
            public void Select() {
                Selected = true;
                BackColor = SelectColor;
            }
            public void DeSelect() {
                Selected = false;
                BackColor = NormColor;
            }
            public fPanel() { }
            public bool Selected { get; set; } = false;
            public Color SelectColor = Color.RoyalBlue;
            public Color NormColor = Color.Navy;
            public string Title { get; set; }
            public Font font { get; set; }
            public Type factory { get; set; }
            public List<qPanel> questions { get; set; } = new List<qPanel>();
            public CheckBox chk { get; set; }

            public fPanel(Type pfactory, string title, Font pFont, EventHandler selectHandler, EventHandler fChkHandler, EventHandler qChkHandler) {
                this.Size = new Size(200, 30);
                this.BorderStyle = BorderStyle.FixedSingle;
                this.Padding = new Padding(0);
                this.Margin = new Padding(0);

                this.Paint += new PaintEventHandler(fPanelPaint);
                this.Click += selectHandler;
                //this.ForeColor=Color.RoyalBlue;
                this.BackColor = NormColor;
                factory = pfactory;
                Title = title;
                font = pFont;

                chk = new CheckBox();
                chk.Location = new Point(7, 2);
                chk.Checked = false;
                chk.Width = 17;
                //chk.Checked=true;
                chk.Click += fChkHandler;
                Controls.Add(chk);

                LoadQuestions(qChkHandler);
            }

            private void LoadQuestions(EventHandler qChkHandler) {
                // -- Load all questions --
                Attribute[] fAtts = Attribute.GetCustomAttributes(factory);
                QuestionList ql = new QuestionList();
                foreach (Attribute fAtt in fAtts)
                    if (fAtt.GetType().Name == "QuestionList")
                        ql = (QuestionList)fAtt;
                string name = string.Empty;
                Type t = typeof(Int32);
                for (int i = 0; i < ql.questions.Count(); i++) { //Decorators swapper-overer.
                    name = ql.questions[i].Name;
                    t = Type.GetType($@"DustBlowerClient.wf{name.Substring(1, name.Length - 1)}, {typeof(wfQuestion).Assembly.GetName().Name}");
                    ql.questions[i] = t;
                }
                Attribute[] qAtts = new Attribute[] { };
                foreach (Type q in ql.questions) {
                    qAtts = Attribute.GetCustomAttributes(q);
                    foreach (Attribute qAtt in qAtts) {
                        if (qAtt.GetType().Name == "NaturalName") {
                            // -- Font + fonts playing up fix ------------------
                            //if( new string[ ]{"x², x³",
                            //                "xⁿ",
                            //                "xᵐ + xⁿ",
                            //                "xᵐ - xⁿ",
                            //                "xᵐ * xⁿ",
                            //                "xᵐ / xⁿ",
                            //                "x⁻ʸ",
                            //                "x ᶠʳᵃᶜᵗⁱᵒⁿ",
                            //                "(xˣ)ʸ",
                            //                "ⁿ√x",
                            //                "√x, ³√x",
                            //                "ⁿ√x",
                            //                "√xʸ",
                            //                "ⁿ√xʸ",
                            //                "√x/y",
                            //                "√xy"
                            //}.Contains( ( ( NaturalName )qAtt ).naturalName ) ) {
                            //    //MessageBox.Show("We working here?");
                            //    Font pFont = new Font( "DejaVu Sans Light", 11 );
                            //    qPanel qp = new qPanel( q, ( ( NaturalName )qAtt ).naturalName, pFont, qChkHandler );
                            //    questions.Add( qp );
                            //} else {
                            //    Font pFont = new Font( "DejaVu Sans Light", 8 );
                            //    qPanel qp = new qPanel( q, ( ( NaturalName )qAtt ).naturalName, pFont, qChkHandler );
                            //    questions.Add( qp );
                            //    //questions.Add(new qPanel(q, ((NaturalName)qAtt).naturalName, font, qChkHandler));
                            //}
                            qPanel qp = new qPanel(q, ((NaturalName)qAtt).naturalName, font, qChkHandler);
                            questions.Add(qp);
                        }
                    }
                }
            }
            private void fPanelPaint(object sender, PaintEventArgs e) {
                Graphics g = this.CreateGraphics();
                g.DrawString(Title, new Font(font.Name, 9), Brushes.White, new Rectangle(23, 8, 180, 20));
                Rectangle r = new Rectangle(ClientRectangle.X, ClientRectangle.Y - 1, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
                g.DrawRectangle(new Pen(Color.DodgerBlue), r);
            }
        }
        public class qPanel : Panel {
            public void Select() {
                Selected = true;
                BackColor = SelectColor;
            }
            public void DeSelect() {
                Selected = false;
                BackColor = NormColor;
            }

            public bool Selected { get; set; } = false;
            private Color SelectColor = Color.RoyalBlue;
            private Color NormColor = Color.Navy;
            public Type Question { get; set; }
            public Font font { get; set; }  //= new Font("DejaVu Sans Light", );
            public CheckBox chk { get; set; }
            public qPanel() { }
            public qPanel(Type question, string text, Font pFont, EventHandler qChkHandler) {
                this.Size = new Size(300, 30);
                this.BorderStyle = BorderStyle.FixedSingle;
                this.Padding = new Padding(0);
                this.Margin = new Padding(0);
                this.ForeColor = Color.White;
                this.BackColor = NormColor;
                this.Paint += new PaintEventHandler(qPanelPaint);
                Question = question;

                chk = new CheckBox();
                chk.Font = pFont;
                chk.Location = new Point(7, 2);
                chk.Checked = false;
                chk.Text = " " + text;
                chk.Width = 300;
                //chk.Checked=true;
                chk.Click += qChkHandler;
                Controls.Add(chk);
            }
            private void qPanelPaint(object sender, PaintEventArgs e) {
                Graphics g = this.CreateGraphics();
                Rectangle r = new Rectangle(ClientRectangle.X, ClientRectangle.Y - 1, ClientRectangle.Width - 2, ClientRectangle.Height - 1);
                g.DrawRectangle(new Pen(Color.DodgerBlue), r);
            }
        }
        private List<Type> SuckEmUpFromTheClassFileUsingReflection() {
            List<Type> rtn = new List<Type>();
            Assembly a = typeof(wfBasicArithmeticFactory).Assembly;
            Type[] fs = a.GetTypes();
            foreach (Type f in fs)
                if (f.GetInterfaces().Contains(typeof(IQuestionFactory)) && Not(f.IsAbstract))
                    rtn.Add(f);
            return rtn;
        }
        private void InitializeComponent() {
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
            this.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            screenWidth = Screen.PrimaryScreen.Bounds.Width;
            screenHeight = Screen.PrimaryScreen.Bounds.Height;
            this.KeyPreview = true;
            this.BackColor = Color.Black;

            factoriesWidth=200;
            questionsWidth=200;
            controlsWidth = factoriesWidth+questionsWidth;

            #region// -- Go! panel --
            goPanel.Location = new Point(screenWidth-controlsWidth, 0);
            goPanel.Size = new Size(controlsWidth, 38);
            goPanel.RowCount = 1;
            goPanel.ColumnCount = 5;
            goPanel.Padding = new Padding(1);
            goPanel.BackColor = Color.Navy;

            questionAreaWidth = (screenWidth-controlsWidth);
            questionAreaHeight = screenHeight-38;

            goBtn = newButton("Go!", hndlGoClick, goPanel);
            stopBtn = newButton("Stop", hndlStopClick, goPanel);

            this.Controls.Add(goPanel);
            #endregion

            #region// -- Controls panel --
            controlsPanel.Location = new Point(0, 0);
            controlsPanel.Size = new Size(questionAreaWidth, 38);
            controlsPanel.RowCount = 1;
            controlsPanel.ColumnCount = 10;
            controlsPanel.Padding = new Padding(1);
            controlsPanel.BackColor = Color.Navy;

            hintsBtn = newButton("Hints", hndlHintsClick, controlsPanel);
            answerBtn = newButton("Answers", hndlAnswersClick, controlsPanel, 80);
            baseBtn = newButton("Base", hndlBaseClick, controlsPanel);
            powerBtn=newButton("Power", hndlPowerClick, controlsPanel);
            rootBtn=newButton("√", hndlRootClick, controlsPanel, 30);
            piBtn=newButton("π", hndlPiClick, controlsPanel, 30);
            this.Controls.Add(controlsPanel);
            #endregion

            #region// -- Factory panel --
            fHousePanel.Location = new Point(screenWidth-controlsWidth, 38);
            fHousePanel.Size = new Size(factoriesWidth, screenHeight);
            fHousePanel.BackColor = Color.Navy;
            fHousePanel.BorderStyle = BorderStyle.FixedSingle;
            fHousePanel.ColumnCount = 1;
            this.Controls.Add(fHousePanel);

            // -- Load all the factory panels --
            List<Type> factoryTypes = SuckEmUpFromTheClassFileUsingReflection();
            string fName = string.Empty;
            for (int i = 0; i < factoryTypes.Count; i++) {
                Attribute[] nl = Attribute.GetCustomAttributes(factoryTypes[i]);
                foreach (Attribute att in nl) {
                    if (att.GetType().Name == "NaturalName")
                        fName = ((NaturalName)att).naturalName;
                }
                fHousePanel.Controls.Add(new fPanel(factoryTypes[i], fName, font, fPanelSelect, fChkSelect, qChkSelect));
            }
            #endregion

            #region// -- Question panel -- 
            qHousePanel.Location = new Point(screenWidth-questionsWidth, 38);
            qHousePanel.Size = new Size(questionsWidth, screenHeight);
            qHousePanel.BackColor = Color.Navy;
            qHousePanel.ColumnCount = 1;
            this.Controls.Add(qHousePanel);
            #endregion
        }
        public Button newButton(string text, EventHandler ev, TableLayoutPanel home, int width=60){
            Button rtn = new Button();
            rtn.Text = text;
            rtn.Height = 30;
            rtn.Width = width;
            rtn.Font = new Font("Verdana", 10);
            rtn.Click += ev;
            rtn.BackColor = Color.RoyalBlue;
            rtn.ForeColor = Color.White;
            home.Controls.Add(rtn);
            return rtn;
        }
        void Form1_Paint(object sender, PaintEventArgs e) {
            if (Backbuffer != null)
                e.Graphics.DrawImageUnscaled(Backbuffer, Point.Empty);
        }
        void Form1_CreateBackBuffer(object sender, EventArgs e) {
            if (Backbuffer != null)
                Backbuffer.Dispose();

            Backbuffer = new Bitmap(ClientSize.Width, ClientSize.Height);
        }
        public void fChkSelect(object sender, EventArgs e) {
            fPanel fpanel = (fPanel)((CheckBox)sender).Parent;

            lock (factories) {
                // update factories List
                if (((CheckBox)sender).Checked) {
                    //Adding
                    factories.Add((qQuestionFactory)fpanel.factory.GetConstructor(new Type[] { }).Invoke(new object[] { }));
                } else {
                    //Removing
                    for (int i = 0; i < factories.Count; i++) {
                        if (factories[i].GetType() == fpanel.factory) {
                            factories.RemoveAt(i);
                            break;
                        }
                    }
                }
            }

            // update GUI
            fPanelSelect(fpanel, new EventArgs());
        }
        public void qChkSelect(object sender, EventArgs e) {
            bool chked = ((CheckBox)sender).Checked;
            if (chked) {
                questionsChecked++;
            } else {
                questionsChecked--;
            }
        }
        public void fPanelSelect(object sender, EventArgs e) {
            fPanel factoryPanel = (fPanel)sender;

            if (factoryPanel.Selected)
                return;

            //Deselect everything, select me
            for (int i = 0; i < fHousePanel.Controls.Count; i++)
                if (fHousePanel.Controls[i].GetType() == typeof(fPanel))
                    ((fPanel)fHousePanel.Controls[i]).DeSelect();

            factoryPanel.Select();

            //Remove all the previous qPanels            
            for (int i = 0; i < qHousePanel.Controls.Count; i++) {
                if (qHousePanel.Controls[i].GetType() == typeof(qPanel)) {
                    qHousePanel.Controls.Remove(qHousePanel.Controls[i]);
                    i--;
                }
            }

            // -- add new panels --
            foreach (qPanel qp in factoryPanel.questions)
                qHousePanel.Controls.Add(qp);

            //Do checkbox click
            factoryPanel.chk.Checked=!factoryPanel.chk.Checked;
            fChkSelect( factoryPanel.chk, new EventArgs());

        }
        // -- Go btns --
        public void hndlGoClick(object sender, EventArgs e) {

            LoadFactories();

            // -- load questions --
            for (int i = 0; i < maxQuestions; i++)
                FetchQuestionBubble();

            stopBtn.Enabled = true;
            goBtn.Enabled = false;
            Draw();
        }
        public void hndlStopClick(object sender, EventArgs e) {
            if (keyJustPressed == "Space")
                return;
            //Task.Factory.StartNew( ( ) => {
            //    Thread.Sleep( 500 );
            //    readyQuestions.Add( qbCancel );
            //} );
            //SomeNamedThreadsStoppingTokenSource.Cancel( );
            lock (factories)
                factories.Clear();
            lock (questions)
                questions.Clear();

            Draw();
            keysIn.Clear();

            stopBtn.Enabled = false;
            goBtn.Enabled = true;

            //Task.Factory.StartNew( ( ) => {
            //    System.Threading.Thread.Sleep( 200 ); //TODO: Can't remember..
            //    TimeTimer.Stop( );
            //    stopBtn.Enabled = false;
            //    goBtn.Enabled = true;
            //} );
        }
        // -- Controls btns --
        public void hndlHintsClick(object sender, EventArgs e) { ShowHints(); }
        public void hndlAnswersClick(object sender, EventArgs e) { ShowAnswers(); }
        public void hndlBaseClick(object sender, EventArgs e){
            states["base"] = !states["base"];
            states["power"] = false;
            Draw();
        }
        public void hndlPowerClick(object sender, EventArgs e) {
            states["power"] = !states["power"];
            states["base"] = false;
            Draw();
        }
        public void hndlRootClick(object sender, EventArgs e) { ProcessInput("√"); }
        public void hndlPiClick(object sender, EventArgs e) { ProcessInput("π"); }

        public void Form1_Click( object sender, MouseEventArgs e ) {
            selection = false;
            //selectedQuestion = blankQuestion;
            questions.ForEach( q=>q.Selected = false );

            foreach( var q in questions ) {
                if( q.Clicked( e ) ) {
                    //MessageBox.Show("1 clicked");
                    selection = true;
                    q.Selected = true;
                    selectedQuestion = q;
                    Invalidate( );
                    Draw( );
                    return;
                }
            }
        }
        public void LoadFactories( ){
            factories.Clear( );
            foreach( Control ctrl in fHousePanel.Controls ) {
                if( ctrl.GetType( ) == typeof( fPanel ) && ( ( fPanel )ctrl ).Selected ) {
                    qQuestionFactory factory = ( qQuestionFactory )( ( fPanel )ctrl ).factory
                                                                              .GetConstructor( new Type[ ] { } )
                                                                              .Invoke( new object[ ] { } );
                    factories.Add( factory );
                }
            }
        }
        public qQuestionFactory  FetchFactory( ){
            lastFactory++;
            if( lastFactory > factories.Count - 1 )
                lastFactory = 0;
            return factories[lastFactory]; //Relying on chk click event to update factories[ ]..
        }
        public fPanel FetchFPanel( qQuestionFactory factory  ){
            fPanel rtn=new fPanel( );
            foreach( Control ctrl in fHousePanel.Controls ) 
                if( ctrl.GetType( ) == typeof( fPanel ) && ( ( fPanel )ctrl ).factory == factory.GetType( ) ) {
                    rtn= ( fPanel )ctrl;
                    break;
                }
            return rtn;
        }
        public qPanel FetchQPanel( fPanel fpanel ){
            qpanels.Clear( );
            //  fetch checked qPanels (from fPanel)
            foreach( qPanel qp in fpanel.questions )
                if( qp.GetType( ) == typeof( qPanel ) && qp.chk.Checked )
                    qpanels.Add( qp );
            if( qpanels.Count == 0 )
                return new qPanel( );
            //  randomly choose
            return qpanels[Utils.R( 1, qpanels.Count ) - 1];
        }
        public IQuestion FetchQuestion( qPanel qpanel, object[] args ){
            return ( IQuestion )qpanel.Question.GetConstructor( args.Select( q => q.GetType( ) ).ToArray( ) ).Invoke( args );
        }
        public void FetchQuestionBubble( ) {
            if( factories.Count == 0 && questions.Count == 0 ) //Must've hit Stop or unselected everything
                return;

            qQuestionFactory factory = FetchFactory( );

            fPanel fpanel = FetchFPanel( factory);

            qPanel qpanel = FetchQPanel( fpanel );

            // DifficultySetting = getOffGui
            qParameters qParams = paramsFactory.getParams( qpanel.Question, DifficultySetting );

            IQuestion nextQuestion = FetchQuestion( qpanel, new object[ ] { 1, qParams });

            nextQuestion.GenerateQuestion( );

            QABubble qb = new QABubble( qParams, nextQuestion, ClientSize.Height, ++id );

            // -- Randomise question location -- 
            Point p = new Point( 0, 0 );
            while( p.X == 0 && p.Y == 0 ) {
                p = new Point( Utils.R( 4, ( questionAreaWidth - ( qb.queWidth - 4 ) ) ),
                              Utils.R( 4+38, ( questionAreaHeight - ( qb.queHeight - 4 ) ) ) );
            }
            qb.SetPosition(p);

            #region // -- Font massage --
            // -- Some manual massaging of fonts because or superscript, linux, font size etc --
            //if( new string[ ]{"x², x³",
            //                "xⁿ",
            //                "xᵐ + xⁿ",
            //                "xᵐ - xⁿ",
            //                "xᵐ * xⁿ",
            //                "xᵐ / xⁿ",
            //                "x⁻ʸ",
            //                "Fraction ˣ",
            //                "x ᶠʳᵃᶜᵗⁱᵒⁿ",
            //                "(xˣ)ʸ",
            //                "ⁿ√x",
            //                "√x, ³√x",
            //                "ⁿ√x",
            //                "√xʸ",
            //                "ⁿ√xʸ",
            //                "√x/y",
            //                "√xy"
            //}.Contains( qpanel.chk.Text.Trim( ) ) || qpanel.chk.Text.Trim( ).Contains( "Standard Form" ) ) {
            //    Font pFont = new Font( "DejaVu Sans Light", 9 );
            //    qb.font = pFont;
            //}
            #endregion

            questions.Add( qb );
        }
        // -- Noise --
        public void shootBlaster( ) {
            //SoundPlayer player = new SoundPlayer(@"/home/havoc/Downloads/Laser.wav");
            //player.PlaySync();

            //System.Diagnostics.Process proc2 = new System.Diagnostics.Process();
            //proc2.EnableRaisingEvents=false; 
            //proc2.StartInfo.FileName = "amixer";
            //proc2.StartInfo.Arguments = "-c 0 -- sset Master playback 50%";
            //proc2.Start();

            System.Diagnostics.Process proc = new System.Diagnostics.Process( );
            proc.EnableRaisingEvents = false;
            proc.StartInfo.FileName = "aplay";
            proc.StartInfo.Arguments = "-t wav /home/havoc/Downloads/Laser.wav";
            proc.Start( );


        }
        public void applaud( ) {
            System.Diagnostics.Process proc = new System.Diagnostics.Process( );
            proc.EnableRaisingEvents = false;
            proc.StartInfo.FileName = "aplay";
            proc.StartInfo.Arguments = "-t wav /home/havoc/Downloads/ModerateApplause2.wav";
            proc.Start( );

        }
        //TODO: This is not a Maths function.
        /// <summary>
        /// Keyboard processing helper function.
        /// </summary>
        /// <returns>A dictionary containing an entry for each key, for status tracking.</returns>
        public static Dictionary<string, bool> KeyStates() {
            Dictionary<string, bool> rtn = new Dictionary<string, bool>();
            rtn.Add("A", false);
            rtn.Add("B", false);
            rtn.Add("C", false);
            rtn.Add("D", false);
            rtn.Add("E", false);
            rtn.Add("F", false);
            rtn.Add("G", false);
            rtn.Add("H", false);
            rtn.Add("I", false);
            rtn.Add("J", false);
            rtn.Add("K", false);
            rtn.Add("L", false);
            rtn.Add("M", false);
            rtn.Add("N", false);
            rtn.Add("O", false);
            rtn.Add("P", false);
            rtn.Add("Q", false);
            rtn.Add("R", false);
            rtn.Add("S", false);
            rtn.Add("T", false);
            rtn.Add("U", false);
            rtn.Add("V", false);
            rtn.Add("W", false);
            rtn.Add("X", false);
            rtn.Add("Y", false);
            rtn.Add("Z", false);
            rtn.Add("Control", false);
            rtn.Add("ControlKey", false);
            rtn.Add("RControlKey", false);
            rtn.Add("LControlKey", false);

            rtn.Add("Alt", false);
            rtn.Add("Escape", false);

            rtn.Add("Shift", false);
            rtn.Add("ShiftKey", false);
            rtn.Add("RShiftKey", false);
            rtn.Add("LShiftKey", false);

            rtn.Add("Delete", false);
            rtn.Add("Enter", false);
            rtn.Add("Return", false);
            rtn.Add("Back", false);
            rtn.Add("Space", false);
            //rtn.Add("",false);
            //rtn.Add("",false);
            //rtn.Add("",false);
            //rtn.Add("",false);
            //rtn.Add("",false);
            //rtn.Add("",false);
            rtn.Add("1", false);
            rtn.Add("2", false);
            rtn.Add("3", false);
            rtn.Add("4", false);
            rtn.Add("5", false);
            rtn.Add("6", false);
            rtn.Add("7", false);
            rtn.Add("8", false);
            rtn.Add("9", false);
            rtn.Add("0", false);
            return rtn;
        }
        #endregion
    }
}
