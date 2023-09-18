using System.Text;

using Aspose.Cells;

using LittleConverter.Models;

using Microsoft.AspNetCore.Http;

namespace LittleConverter.Converters
{
    /// <summary>
    /// It's only have one method : <c>ConvertItAsync</c> <para></para>
    /// to convert the selected file to the desired Type
    /// </summary>
    public static class ConvertFromIFormFile
    {
	   /// <summary>
	   /// asdfsdfasdfasd
	   /// </summary>
	   /// <param name="convertRequest"></param>
	   /// <returns>String as Json file</returns>
	   public static async Task<string> ConvertItAsync( ConvertRequest convertRequest )
	   {
		  SaveFormat saveType = Enum.GetValues( typeof( SaveFormat ) ).OfType<SaveFormat>()
									   .FirstOrDefault( x => x.ToString() == convertRequest.SaveFormat.ToString() );
		  if( !convertRequest.SaveConvertedFile )
		  {
			 return ConvertTo( convertRequest.BaseFile, string.Empty, saveType );
		  }

		  //~\wwwroot
		  string wwwrootPath = Path.Combine( Directory.GetCurrentDirectory(), "wwwroot" );

		  //~\wwwroot\ConvertedFiles\NewFolderName\JSON
		  string convertedFilesRootPath = Path.Combine( wwwrootPath, "ConvertedFiles", convertRequest.ConvertedFilePath, convertRequest.SaveFormat.ToString() );
		  string convertedFileSavePath = string.Empty;
		  string baseFileExtension = Path.GetExtension( convertRequest.BaseFile.FileName.ToUpper() );

		  //~\wwwroot\BaselFiles\NewFolderName\XLSX
		  string baseFilesPath = Path.Combine( wwwrootPath, "BaseFiles", convertRequest.ConvertedFilePath, baseFileExtension.TrimStart( '.' ) );
		  string baseFileUploadPath = string.Empty;

		  //Set the new name to the converted file if the NewFilename property is set by the user
		  if( !string.IsNullOrEmpty( convertRequest.NewFileName ) && !string.IsNullOrWhiteSpace( convertRequest.NewFileName ) )
		  {
			 baseFileUploadPath = Path.Combine( baseFilesPath, convertRequest.NewFileName + baseFileExtension );
			 convertedFileSavePath = Path.Combine( convertedFilesRootPath, convertRequest.NewFileName );
		  }
		  //Otherwise, the same name as the base file will be used for the converted file
		  else
		  {
			 //IFormFile.FileName itself includes the file extension
			 baseFileUploadPath = Path.Combine( baseFilesPath, convertRequest.BaseFile.FileName );

			 int indextOfDot = convertRequest.BaseFile.FileName.IndexOf( '.' );
			 convertedFileSavePath = Path.Combine( convertedFilesRootPath, convertRequest.BaseFile.FileName.Remove( indextOfDot ) );
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

		  ConvertTo( convertRequest.BaseFile, convertedFileSavePath, saveType );
		  return File.ReadAllText( convertedFileSavePath );
	   }

	   private static string ConvertTo( IFormFile baseFile, string convertedFileSavePath, SaveFormat saveType )
	   {
		  if( string.IsNullOrEmpty( convertedFileSavePath ) )
			 using( Stream fileStream = baseFile.OpenReadStream() )
			 {
				Workbook workbook = new Workbook( fileStream );
				MemoryStream mStream = new MemoryStream();

				workbook.Save( mStream, saveType );
				return Encoding.UTF8.GetString( mStream.ToArray() );
			 }

		  using( Stream fileStream = baseFile.OpenReadStream() )
		  {
			 Workbook workbook = new Workbook( fileStream );
			 workbook.Save( convertedFileSavePath + saveType.ToString().ToLower() );
		  }
		  return string.Empty;
	   }
    }
}