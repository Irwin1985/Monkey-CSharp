using System;
using System.Linq;
using System.Collections.Generic;

namespace Monkey.Core
{
    public static class MonkeyBuiltins
    {
        public static readonly Dictionary<string, MonkeyBuiltin> Builtins = new Dictionary<string, MonkeyBuiltin>();

        static MonkeyBuiltins()
        {
            Builtins.Add("len", new MonkeyBuiltin { Fn = Len });
            Builtins.Add("first", new MonkeyBuiltin { Fn = First });
            Builtins.Add("last", new MonkeyBuiltin { Fn = Last });
            Builtins.Add("rest", new MonkeyBuiltin { Fn = Rest });
            Builtins.Add("push", new MonkeyBuiltin { Fn = Push });
            Builtins.Add("puts", new MonkeyBuiltin { Fn = Puts });
        }

        private static IMonkeyObject Len(List<IMonkeyObject> args)
        {
            if (args.Count != 1)
                return Evaluator.NewError($"Wrong number of arguments. Got={args.Count}, want=1");
            if (args[0] is MonkeyString s)
                return new MonkeyInteger { Value = s.Value.Length };
            if (args[0] is MonkeyArray a)
                return new MonkeyInteger { Value = a.Elements.Count };            
            return Evaluator.NewError($"Argument to 'len' not supported. Got {args[0].Type}");
        }

        private static IMonkeyObject First(List<IMonkeyObject> args)
        {
            if (args.Count != 1)
                return Evaluator.NewError($"Wrong number of arguments. Got={args.Count}, want=1");
            if (args[0] is MonkeyArray arr)
                return arr.Elements.Count > 0 ? arr.Elements[0] : Evaluator.Null;
            return Evaluator.NewError($"Argument to 'first' must be Array. Got {args[0].Type}");
        }

        private static IMonkeyObject Last(List<IMonkeyObject> args)
        {
            if (args.Count != 1)
                return Evaluator.NewError($"Wrong number of arguments. Got={args.Count}, want=1");
            if (!(args[0] is MonkeyArray arr))
                return Evaluator.NewError($"Argument to 'last' must be Array. Got {args[0].Type}");
            var length = arr.Elements.Count;
            return length > 0 ? arr.Elements[length - 1] : Evaluator.Null;
        }

        private static IMonkeyObject Rest(List<IMonkeyObject> args)
        {
            if (args.Count != 1)
                return Evaluator.NewError($"Wrong number of arguments. Got={args.Count}, want=1");
            if (!(args[0] is MonkeyArray arr))
                return Evaluator.NewError($"Argument to 'last' must be Array. Got {args[0].Type}");
            var length = arr.Elements.Count;
            if (length > 0)
                return new MonkeyArray { Elements = arr.Elements.Skip(1).ToList() };
            return Evaluator.Null;
        }
        
        private static IMonkeyObject Push(List<IMonkeyObject> args)
        {
            if (args.Count != 2)
                return Evaluator.NewError($"Wrong number of arguments. Got={args.Count}, want=2");
            if (!(args[0] is MonkeyArray arr))
                return Evaluator.NewError($"Argument to 'push' must be Array. Got {args[0].Type}");
            var newElements = arr.Elements.Skip(0).ToList();
            newElements.Add(args[1]);
            return new MonkeyArray { Elements = newElements };
        }        

        private static IMonkeyObject Puts(List<IMonkeyObject> args)
        {
            foreach (var arg in args)
                Console.WriteLine(arg.Inspect());
            return Evaluator.Null;
        }
    }
}