using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Trello.DAL.Models;

namespace Trello.Controllers
{
    public class FilesController : Controller
    {
        DataModel db = new DataModel();
        public  ActionResult GetFile(int id)
        {
            var imageData = db.Attachments.First((i) => i.Id == id);
            string extension = Path.GetExtension(imageData.FileName);

            switch (extension.ToLower())
            {
                case ".xls":
                    return File("~/images/excel.png", "application/octet-stream");
                case ".torrent":
                    return File("~/images/uTorrent.png", "application/octet-stream");
                case ".xlsx":
                    return File("~/images/excel.png", "application/octet-stream");
                case ".doc":
                    return File("~/images/word.png", "application/octet-stream");
                case ".docx":
                    return File("~/images/word.png", "application/octet-stream");
                case ".js":
                    return File("~/images/javascript.png", "application/octet-stream");
                case ".zip":
                    return File("~/images/zip.png", "application/octet-stream");
                case ".pdf":
                    return File("~/images/pdf.png", "application/octet-stream");
                case ".htm":
                    return File("~/images/html.png", "application/octet-stream");
                case ".html":
                    return File("~/images/html.png", "application/octet-stream");
                case ".css":
                    return File("~/images/css3.png", "application/octet-stream");
                case ".txt":
                    return File("~/images/Notepad.png", "application/octet-stream");
                case ".dat":
                    return File("~/images/Notepad.png", "application/octet-stream");
                case ".log":
                    return File("~/images/Notepad.png", "application/octet-stream");
                case ".xml":
                    return File("~/images/Notepad.png", "application/octet-stream");
                case ".json":
                    return File("~/images/Notepad.png", "application/octet-stream");
                case ".dll":
                    return File("~/images/dll.png", "application/octet-stream");
                case ".png":
                    return File(imageData.FileContent, "application/octet-stream");
                case ".jpg":
                    return File(imageData.FileContent, "application/octet-stream");
                case ".jpeg":
                    return File(imageData.FileContent, "application/octet-stream");
                case ".ico":
                    return File(imageData.FileContent, "application/octet-stream");
                case ".bmp":
                    return File(imageData.FileContent, "application/octet-stream");
                case ".gif":
                    return File(imageData.FileContent, "application/octet-stream");
                default:
                    return File("~/images/Notepad.png", "application /octet-stream");
            }
        }


        public ActionResult DownloadFile(int id)
        {
            var file = db.Attachments.First((i) => i.Id == id);
            var cd = new System.Net.Mime.ContentDisposition
            {
                 FileName = file.FileName,
                 Inline = false,
            };
            
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(file.FileContent, "application/octet-stream");
        }


        public void Resize(string imageFile, string outputFile, double scaleFactor, ImageFormat imageFormat)
        {
            using (var srcImage = Image.FromFile(imageFile))
            {
                var newWidth = (int)(srcImage.Width * scaleFactor);
                var newHeight = (int)(srcImage.Height * scaleFactor);
                using (var newImage = new Bitmap(newWidth, newHeight))
                using (var graphics = Graphics.FromImage(newImage))
                {
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.DrawImage(srcImage, new Rectangle(0, 0, newWidth, newHeight));
                    newImage.Save(outputFile, imageFormat);
                }
            }
        }
    }
}