namespace DPSIW.Common.Models;

public record MedicalNotesMetadata(
    string type = "url",
    string content = "",
    string? filePath = null,
    string? fileId = null,
    string? fileUrl = null,
    string? blobName = null
);

public record MedicalNotes(
    string id,
    string pid,
    string cid,
    string type,
    MedicalNotesMetadata metadata, // Alternatively, create a base class/interface for metadata types    
    DateTime created
);

public record Message(
    string type
);
