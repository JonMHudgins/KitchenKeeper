CREATE PROCEDURE UploadPDF (@ItemID int, @Content varbinary(max))
AS
BEGIN
  INSERT INTO PDFs (ItemID, Content) VALUES (@ItemID, @Content)
END
GO

CREATE PROCEDURE UploadJSON (@ItemID int, @Content varbinary(max))
AS
BEGIN
  INSERT INTO JSONs (ItemID, Content) VALUES (@ItemID, @Content)
END
GO