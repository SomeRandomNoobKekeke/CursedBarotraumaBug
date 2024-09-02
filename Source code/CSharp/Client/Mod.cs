using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

[assembly: IgnoresAccessChecksTo("Barotrauma")]
[assembly: IgnoresAccessChecksTo("DedicatedServer")]
[assembly: IgnoresAccessChecksTo("BarotraumaCore")]

namespace Cursed
{
  public partial class Mod : IAssemblyPlugin
  {
    public Harmony harmony;

    public static Dictionary<MapEntity, int> justANormalDict = new Dictionary<MapEntity, int>();
    public static int howManyTimesItWasNull = 1;
    public void Initialize()
    {
      harmony = new Harmony("cursed");

      if (justANormalDict == null)
      {
        log($"justANormalDict was null in Initialize", Color.Yellow);
        justANormalDict = new Dictionary<MapEntity, int>();
        howManyTimesItWasNull++;
      }


      patchAll();
    }

    public static void attachUselessData(MapEntity __instance)
    {
      if (justANormalDict == null)
      {
        log($"justANormalDict is null in MapEntity constructor for {howManyTimesItWasNull} time", Color.Yellow);
        justANormalDict = new Dictionary<MapEntity, int>();
        howManyTimesItWasNull++;
      }

      justANormalDict[__instance] = 123;

    }

    public void patchAll()
    {
      harmony.Patch(
        original: typeof(MapEntity).GetConstructors()[0],
        postfix: new HarmonyMethod(typeof(Mod).GetMethod("attachUselessData"))
      );
    }


    public static void log(object msg, Color? cl = null, [CallerLineNumber] int lineNumber = 0)
    {
      if (cl == null) cl = Color.Cyan;
      DebugConsole.NewMessage($"{lineNumber}| {msg ?? "null"}", cl);
    }

    public void OnLoadCompleted() { }
    public void PreInitPatching() { }

    public void Dispose()
    {
      harmony.UnpatchAll(harmony.Id);
      harmony = null;

      justANormalDict = null;
    }
  }
}