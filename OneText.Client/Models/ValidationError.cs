using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneText.Client.Models;
public record ValidationError(string Property, List<string> Errors)
{
}
