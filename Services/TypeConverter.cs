using System.Text;

using Aspose.Cells;

using LittleConverter.Models;

namespace SomeTypesConverter.Services
{
    /// <summary>
    /// It's only have one method : <c>ConvertItAsync</c> <para></para>
    /// to convert the selected file to the desired Type
    /// </summary>
    public static class TypeConverter
    {
	   /// <summary>
	   /// 
	   /// </summary>
	   /// <param name="convertRequest"></param>
	   /// <returns>string as json file</returns>
	   public static async Task<string> ConvertItAsync( ConvertRequest convertRequest )
	   {
		  if( !convertRequest.SaveConvertedFile )
		  {
			 using( Stream fileStream = convertRequest.BaseFile.OpenReadStream() )
			 {
				Workbook workbook = new Workbook( fileStream );
				MemoryStream mStream = new MemoryStream();
				workbook.Save( mStream, saveFormat: SaveFormat.Json );
				return Encoding.ASCII.GetString( mStream.ToArray() );
			 }
		  }

		  //~\wwwroot
		  string wwwrootPath = Path.Combine( Directory.GetCurrentDirectory(), "wwwroot" );

		  //~\wwwroot\ConvertedFiles\NewFolderName\JSON
		  string convertedFilesRootPath = Path.Combine( wwwrootPath, "ConvertedFiles", convertRequest.ConvertedFilePath, convertRequest.ConvertToType.ToString() );
		  string convertedFileSavePath = string.Empty;
		  string basicFileExtension = Path.GetExtension( convertRequest.BaseFile.FileName.ToUpper() );

		  //~\wwwroot\BaselFiles\NewFolderName\XLSX
		  string basicFilesPath = Path.Combine( wwwrootPath, "BaseFiles", convertRequest.ConvertedFilePath, basicFileExtension.TrimStart( '.' ) );
		  string basicFileUploadPath = string.Empty;

		  //Set the new name to the converted file if the NewFilename property is set by the user
		  if( !string.IsNullOrEmpty( convertRequest.NewFileName ) && !string.IsNullOrWhiteSpace( convertRequest.NewFileName ) )
		  {
			 basicFileUploadPath = Path.Combine( basicFilesPath, convertRequest.NewFileName + basicFileExtension );
			 convertedFileSavePath = Path.Combine( convertedFilesRootPath, convertRequest.NewFileName + "." + convertRequest.ConvertToType.ToString().ToLower() );
		  }
		  //Otherwise, the same name as the base file will be used for the converted file
		  else
		  {
			 basicFileUploadPath = Path.Combine( basicFilesPath, convertRequest.BaseFile.FileName + basicFileExtension );
			 convertedFileSavePath = Path.Combine( convertedFilesRootPath, convertRequest.BaseFile.FileName + "." + convertRequest.ConvertToType.ToString().ToLower() );
		  }

		  if( convertRequest.UploadBaseFile )
		  {
			 if( !Directory.Exists( basicFilesPath ) )
				Directory.CreateDirectory( basicFilesPath );
			 using( FileStream stream = new FileStream( basicFileUploadPath, FileMode.Create ) )
			 {
				await convertRequest.BaseFile.CopyToAsync( stream );
			 }
		  }

		  if( !Directory.Exists( convertedFilesRootPath ) )
			 Directory.CreateDirectory( convertedFilesRootPath );


		  using( Stream fileStream = convertRequest.BaseFile.OpenReadStream() )
		  {
			 Workbook workbook = new Workbook( fileStream );
			 workbook.Save( convertedFileSavePath );
		  }
		  return File.ReadAllText( convertedFileSavePath );
	   }
    }
}