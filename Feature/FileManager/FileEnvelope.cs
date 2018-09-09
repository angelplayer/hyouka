
using System.Collections.Generic;

namespace hyouka_api.Feature.FileManger
{
  public class FileData
  {
    public string Name { get; set; }
    public string Rights { get; set; }
    public string Size { get; set; }
    public string Date { get; set; }
    public string Type { get; set; }

    public FileData(string name, string size, string date, string type, string right)
    {
      this.Name = name;
      this.Size = size;
      this.Date = date;
      this.Type = type;
      this.Rights = right;
    }
  }

  public class FileResultEnvelope
  {
    public List<FileData> Result { get; set; }

    public FileResultEnvelope(List<FileData> data)
    {
      this.Result = data;
    }
  }

  public class FileActionResult
  {
    public bool Success { get; set; }
    public System.Object Error { get; set; }
  }

  public class ActionResultEnvelope
  {
    public FileActionResult Result { get; set; }

    public ActionResultEnvelope(FileActionResult result)
    {
      this.Result = result;
    }
  }

  public class ContentEnvelope
  {
    public string Result { get; set; }
  }

  public class ActionCommand
  {
    public string Action { get; set; }
    public string Path { get; set; }
    public string NewPath { get; set; }
    public string Item { get; set; }
    public string newItemPath { get; set; }
    public string[] Items { get; set; }
  }
}