using System.Text;

using Aspose.Cells;

using LittleConverter.Models;

namespace SomeTypesConverter.Services
{
    /// <summary>
    /// It's only have one method : <c>ConvertItAsync</c> <para></para>
    /// to convert the selected file to the desired Type
    /// </summary>
    public static class ConvertIt
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
		  string baseFileExtension = Path.GetExtension( convertRequest.BaseFile.FileName.ToUpper() );

		  //~\wwwroot\BaselFiles\NewFolderName\XLSX
		  string baseFilesPath = Path.Combine( wwwrootPath, "BaseFiles", convertRequest.ConvertedFilePath, baseFileExtension.TrimStart( '.' ) );
		  string baseFileUploadPath = string.Empty;

		  //Set the new name to the converted file if the NewFilename property is set by the user
		  if( !string.IsNullOrEmpty( convertRequest.NewFileName ) && !string.IsNullOrWhiteSpace( convertRequest.NewFileName ) )
		  {
			 baseFileUploadPath = Path.Combine( baseFilesPath, convertRequest.NewFileName + baseFileExtension );
			 convertedFileSavePath = Path.Combine( convertedFilesRootPath, convertRequest.NewFileName + convertRequest.ConvertToType.ToString().ToLower() );
		  }
		  //Otherwise, the same name as the base file will be used for the converted file
		  else
		  {
			 //IFormFile.FileName itself includes the file extension
			 baseFileUploadPath = Path.Combine( baseFilesPath, convertRequest.BaseFile.FileName );

			 int indextOfDot = convertRequest.BaseFile.FileName.IndexOf( '.' );
			 convertedFileSavePath = Path.Combine( convertedFilesRootPath, convertRequest.BaseFile.FileName.Remove( indextOfDot ) + "." + convertRequest.ConvertToType.ToString().ToLower() );
		  }

		  if( convertRequest.UploadBaseFile )
		  {
			 if( !Directory.Exists( baseFilesPath ) )
				Directory.CreateDirectory( baseFilesPath );
			 using( FileStream stream = new FileStream( baseFileUploadPath, FileMode.Create ) )
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