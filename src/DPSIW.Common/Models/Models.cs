using System.Text.Json;

namespace DPSIW.Common.Models;

public record Message(string type, string pid, string cid, JsonElement metadata);
public record Metadata(string file_url, string blob_id);
public record MedicalNoteAgent(string type, string pid, string cid, Metadata metadata);
