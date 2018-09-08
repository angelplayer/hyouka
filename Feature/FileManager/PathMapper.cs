using System.IO;

namespace hyouka_api.Feature.FileManger
{
  public interface IPathMapper
  {
    string BasePath { get; set; }
    string MapPath(string path);
  }

  public class PathMapper : IPathMapper
  {
    public string BasePath { get; set; }
    public string RootPath { get; set; }

    public PathMapper(string rootPath, string basePath)
    {
      this.BasePath = basePath;
      this.RootPath = rootPath;
    }

    public string MapPath(string path)
    {
      return Path.Combine(this.RootPath, this.BasePath, path.TrimStart(new char[] { ' ', '/' })); //remove leading /
    }
  }
}