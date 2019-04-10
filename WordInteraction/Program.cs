using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;

namespace WordInteraction
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            var wordApp = new Application();
            wordApp.Visible = false;
            */
            Application app = (Application)Marshal.GetActiveObject("Word.Application");
            var documents = app.Documents;
            foreach (var item in documents)
            {
                Console.WriteLine("Document is opened");
            }
            Range rng = app.Selection.Range;
            //rng.Bold = 1;
            
            rng.Text +="Test string1 from C#";

            rng.Text += "Test string 2";
           
            rng.FormattedText.Font.Bold = 1;
            rng.Text += "String 3";
            //rng.FormattedText.Font.Bold = 0;

            //rng.Select();
            Range rng2 = app.Selection.Range;
            
            rng2.Text ="Test new string from C#";
            rng2.Bold = 1;

            
            Selection sel = app.Selection;
            sel.TypeText(rng2.Text);
            //sel.BoldRun();
            sel.TypeText("2");
            //sel.BoldRun();
            
            sel.TypeText("3 ");
            
            //SelectionInsertText();
        }
        /*
        private static void SelectionInsertText()
        {
            Word.Selection currentSelection = Application.Selection;

            // Store the user's current Overtype selection
            bool userOvertype = Application.Options.Overtype;

            // Make sure Overtype is turned off.
            if (Application.Options.Overtype)
            {
                Application.Options.Overtype = false;
            }

            // Test to see if selection is an insertion point.
            if (currentSelection.Type == Word.WdSelectionType.wdSelectionIP)
            {
                currentSelection.TypeText("Inserting at insertion point. ");
                currentSelection.TypeParagraph();
            }
            else
                if (currentSelection.Type == Word.WdSelectionType.wdSelectionNormal)
            {
                // Move to start of selection.
                if (Application.Options.ReplaceSelection)
                {
                    object direction = Word.WdCollapseDirection.wdCollapseStart;
                    currentSelection.Collapse(ref direction);
                }
                currentSelection.TypeText("Inserting before a text block. ");
                currentSelection.TypeParagraph();
            }
            else
            {
                // Do nothing.
            }

            // Restore the user's Overtype selection
            Application.Options.Overtype = userOvertype;
        }
        */
    }
}
