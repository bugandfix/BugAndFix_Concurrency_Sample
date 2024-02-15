using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugAndFix_Concurrency_Sample.Entities;

public class Package
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PackageID { get; set; }

    public required string Name { get; set; }

    public decimal Width { get; set; }

    public decimal Height { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = new byte[8];
}
