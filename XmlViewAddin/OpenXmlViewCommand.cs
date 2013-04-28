using System;
using HP.Utt.UttCore;
using HP.Utt.CodeEditorUtils;
using ICSharpCode.SharpDevelop.Editor;
using System.Text;
using System.Xml;
using HP.Utt.UttDialog;
using HP.LR.VuGen.XmlViewer;

namespace XmlViewAddin
{
  public class OpenXmlViewCommand : UttBaseWpfCommand
  {
    public static bool IsValidXml(string xml)
    {
      XmlDocument doc = new XmlDocument();
      try
      {
        doc.LoadXml(xml);
      }
      catch
      {
        return false;
      }

      return true;
    }

    public override void Run()
    {
      ITextEditor editor = UttCodeEditor.GetActiveTextEditor();
      if (editor == null)
        return;
      string[] lines = editor.SelectedText.Split(new string[]{Environment.NewLine},StringSplitOptions.RemoveEmptyEntries);
      StringBuilder result = new StringBuilder();
      foreach (string line in lines) 
      {
        string proccessedLine = line.Trim();
        //Remove the leading and trailing " - this is VuGen specific code
        if (proccessedLine.StartsWith("\""))
        {
          proccessedLine = proccessedLine.Remove(0, 1);
        }
        if (proccessedLine.EndsWith("\""))
        {
          proccessedLine = proccessedLine.Remove(proccessedLine.Length-1, 1);
        }
        proccessedLine = proccessedLine.Replace("\\","");
        
        result.Append(proccessedLine);
      }
      bool isValid = IsValidXml(result.ToString());
      if (isValid)
      {
        ShowXmlDialog(result.ToString());
      }
    }

    private void ShowXmlDialog(string xml)
    {
      CustomDialog dialog = new CustomDialog();
      XmlViewSingleContent content = new XmlViewSingleContent();
      SingleDirectionData data = new SingleDirectionData();
      content.DataContext = data;
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(xml);
      data.Document = doc;
      dialog.Content = content;
      dialog.Show();
    }
  }
}
