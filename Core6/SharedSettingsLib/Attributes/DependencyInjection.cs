using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedSettingsLib.Attributes;
  /// <summary>
  /// Inject-AddScoped
  /// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)] public class Inject : Attribute { }

/// <summary>
/// InjectSingleton-AddSingleton
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)] public class InjectSingleton : Attribute { }

/// <summary>
/// InjectScoped-AddScoped
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)] public class InjectScoped : Attribute { }
/// <summary>
/// InjectTransient-AddTransient
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)] public class InjectTransient : Attribute { }