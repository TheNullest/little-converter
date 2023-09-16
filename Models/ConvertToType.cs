using System.Text.Json.Serialization;

namespace LittleConverter.Models
{
    [JsonConverter( typeof( JsonStringEnumConverter ) )]
    public enum ConvertToType
    {
	   XLSX = 1,
	   XLS,
	   XML,
	   XLSM,
	   XLSB,
	   XLT,
	   PDF,
	   DOCX,
	   PPTX,
	   ODS,
	   OTS,
	   CSV,
	   TSV,
	   HTML,
	   MHTML,
	   HTM,
	   MHT,
	   NUMBERS,
	   JPG,
	   PNG,
	   WEBP,
	   SVG,
	   TIFF,
	   XPS,
	   MD,
	   JSON,
	   ZIP,
	   SQL,
	   TXT,
	   ET,
	   ETT,
	   PRN,
	   DIF,
	   EMF,
	   FODS,
	   GIF,
	   XLAM,
	   XLTM,
	   XLTX
    }
}
