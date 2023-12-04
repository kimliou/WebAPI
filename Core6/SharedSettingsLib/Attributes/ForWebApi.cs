using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedSettingsLib.Attributes;

[AttributeUsage(AttributeTargets.All, Inherited = false)] public class AllowAnyUrlRouting : Attribute { }