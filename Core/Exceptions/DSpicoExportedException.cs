using System;
using System.Collections.Generic;
using System.Text;

namespace DspicoThemeForms.Core.Exceptions;

public class DSpicoExportedException : Exception
{
    public DSpicoExportedException() : base("This looks like an exported DSpico theme. We only supported for the DSpico theme source (which means the png files).")
    {
    }
    public DSpicoExportedException(string message) : base(message)
    {
    }
    public DSpicoExportedException(string message, Exception inner) : base(message, inner)
    {
    }
}
