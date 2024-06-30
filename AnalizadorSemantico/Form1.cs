using Irony.Parsing;

namespace AnalizadorSemantico
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            var grammar = new SimpleGrammar();
            var parser = new Parser(grammar);
            var parseTree = parser.Parse(txtInput.Text);

            if (parseTree.HasErrors())
            {
                txtOutput.Text = "Syntax errors found:\n";
                foreach (var error in parseTree.ParserMessages)
                {
                    txtOutput.AppendText($"{error.Message}\n");
                }
            }
            else
            {
                txtOutput.Text = "Parse successful!";
            }
        }
    }

    public class SimpleGrammar : Grammar
    {
        public SimpleGrammar()
        {
            var number = new NumberLiteral("number");
            var identifier = new IdentifierTerminal("identifier");

            var expr = new NonTerminal("expr");
            var binExpr = new NonTerminal("binExpr");
            var term = new NonTerminal("term");
            var factor = new NonTerminal("factor");
            var statement = new NonTerminal("statement");
            var assignment = new NonTerminal("assignment");

            expr.Rule = term | binExpr;
            binExpr.Rule = expr + "+" + term
                         | expr + "-" + term
                         | expr + "*" + term
                         | expr + "/" + term;
            term.Rule = factor;
            factor.Rule = number | identifier | "(" + expr + ")";
            assignment.Rule = identifier + "=" + expr;

            statement.Rule = assignment | expr;

            this.Root = statement;
            this.RegisterOperators(1, "+", "-");
            this.RegisterOperators(2, "*", "/");
            this.MarkPunctuation("(", ")");
        }
    }
}
