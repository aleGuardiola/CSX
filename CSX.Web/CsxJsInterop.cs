using CSX.Rendering;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WebAssembly.JSInterop;

namespace CSX.Web
{
    public static class CsxJsInterop
    {
        static Action<Event>? _handler;

        public static void CreateElement(string tag, string id)
        {
            var callInfo = new JSCallInfo()
            {
                FunctionIdentifier = nameof(CreateElement),
                ResultType = JSCallResultType.JSVoidResult,
                TargetInstanceId = 0,
            };

            InternalCalls.InvokeJS<string, string, string, string>(out var exception, ref callInfo, tag, id, null);

            if (exception != null)
            {
                Console.WriteLine(exception);
            }
        }

        public static void RemoveElement(string id)
        {
            var callInfo = new JSCallInfo()
            {
                FunctionIdentifier = nameof(RemoveElement),
                ResultType = JSCallResultType.JSVoidResult,
                TargetInstanceId = 0,
            };

            InternalCalls.InvokeJS<string, string, string, string>(out var exception, ref callInfo, id, null, null);
            if (exception != null)
            {
                Console.WriteLine(exception);
            }
        }

        public static void DestroyElement(string id)
        {
            var callInfo = new JSCallInfo()
            {
                FunctionIdentifier = nameof(DestroyElement),
                ResultType = JSCallResultType.JSVoidResult,
                TargetInstanceId = 0,
            };

            InternalCalls.InvokeJS<string, string, string, string>(out var exception, ref callInfo, id, null, null);
            if (exception != null)
            {
                Console.WriteLine(exception);
            }
        }


        public static void SetElementAttribute(string id, string name, string value)
        {
            var callInfo = new JSCallInfo()
            {
                FunctionIdentifier = nameof(SetElementAttribute),
                ResultType = JSCallResultType.JSVoidResult,
                TargetInstanceId = 0,
            };

            InternalCalls.InvokeJS<string, string, string, string>(out var exception, ref callInfo, id, name, value);
            if (exception != null)
            {
                Console.WriteLine(exception);
            }
        }


        public static void AttachElement(string parentId, string id)
        {
            var callInfo = new JSCallInfo()
            {
                FunctionIdentifier = nameof(AttachElement),
                ResultType = JSCallResultType.JSVoidResult,
                TargetInstanceId = 0,
            };

            InternalCalls.InvokeJS<string, string, string, string>(out var exception, ref callInfo, parentId, id, null);
            if (exception != null)
            {
                Console.WriteLine(exception);
            }
        }


        public static void SetElementText(string id, string text)
        {
            var callInfo = new JSCallInfo()
            {
                FunctionIdentifier = nameof(SetElementText),
                ResultType = JSCallResultType.JSVoidResult,
                TargetInstanceId = 0,
            };

            InternalCalls.InvokeJS<string, string, string, string>(out var exception, ref callInfo, id, text, null);
            if (exception != null)
            {
                Console.WriteLine(exception);
            }
        }

        public static void SetChildren(string id, string childrenJson)
        {
            var callInfo = new JSCallInfo()
            {
                FunctionIdentifier = nameof(SetChildren),
                ResultType = JSCallResultType.JSVoidResult,
                TargetInstanceId = 0,
            };

            InternalCalls.InvokeJS<string, string, string, string>(out var exception, ref callInfo, id, childrenJson, null);
            if (exception != null)
            {
                Console.WriteLine(exception);
            }
        }

        public static void SetElementAttributes(string id, string attrs)
        {
            var callInfo = new JSCallInfo()
            {
                FunctionIdentifier = nameof(SetElementAttributes),
                ResultType = JSCallResultType.JSVoidResult,
                TargetInstanceId = 0,
            };

            InternalCalls.InvokeJS<string, string, string, string>(out var exception, ref callInfo, id, attrs, null);
            if (exception != null)
            {
                Console.WriteLine(exception);
            }
        }

        public static void SetEventHandler(Action<Event> handler)
        {
            _handler = handler;
        }

        [JSInvokable]
        public static void OnEvent(Event @event)
        {
            if (_handler == null)
            {
                return;
            }

            lock (_handler)
            {
                _handler(@event);
            }
        }
    }


}


namespace WebAssembly.JSInterop
{

    // copied from https://github.com/dotnet/aspnetcore/blob/c85baf8db0c72ae8e68643029d514b2e737c9fae/src/Components/WebAssembly/JSInterop/src/InternalCalls.cs

    /// <summary>
    /// Methods that map to the functions compiled into the Mono WebAssembly runtime,
    /// as defined by 'mono_add_internal_call' calls in driver.c.
    /// </summary>

    internal static class InternalCalls
    {
        // The exact namespace, type, and method names must match the corresponding entries
        // in driver.c in the Mono distribution
        /// See: https://github.com/mono/mono/blob/90574987940959fe386008a850982ea18236a533/sdks/wasm/src/driver.c#L318-L319

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern TRes InvokeJS<T0, T1, T2, TRes>(out string exception, ref JSCallInfo callInfo, [AllowNull] T0 arg0, [AllowNull] T1 arg1, [AllowNull] T2 arg2);
    }

    [StructLayout(LayoutKind.Explicit, Pack = 4)]
    internal struct JSCallInfo
    {
        [FieldOffset(0)]
        public string FunctionIdentifier;

        [FieldOffset(4)]
        public JSCallResultType ResultType;

        [FieldOffset(8)]
        public string MarshalledCallArgsJson;

        [FieldOffset(12)]
        public long MarshalledCallAsyncHandle;

        [FieldOffset(20)]
        public long TargetInstanceId;
    }
}