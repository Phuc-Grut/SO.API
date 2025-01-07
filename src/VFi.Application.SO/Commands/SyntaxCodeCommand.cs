using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using VFi.Application.SO.Commands.Validations;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class SyntaxCodeCommand : Command
{
}

public class UseCodeCommand : SyntaxCodeCommand
{
    public string Syntax { get; set; }
    public string Code { get; set; }

    public UseCodeCommand(string syntax, string code)
    {
        Syntax = syntax;
        Code = code;
    }
}
