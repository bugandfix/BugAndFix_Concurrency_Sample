using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugAndFix_Concurrency_Sample.Entities;


public class Shipment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ShipmentID { get; set; }
    public required string AgentName { get; set; }
    public int ParcelID { get; set; }
    public DateTime ShipmentDate { get; set; }
    public DateTime ShipmentInDate { get; set; }
    public DateTime ShipmentOutDate { get; set; }

    // ConcurrencyCheck is specifies that this -
    // property participates in optimistic concurrency checking.
    [ConcurrencyCheck]
    public Guid RecordVersion { get; set; }
}

