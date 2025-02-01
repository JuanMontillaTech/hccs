using System;
using System.Collections.Generic;

namespace ERP.Domain.Dtos;

public class UpdateReceiptStatusRequest
{
    public List<Guid> ReceiptIds { get; set; }
    public Guid NewStatusId { get; set; }
}