using CSX.Rendering;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WebAssembly.JSInterop;

namespace CSX.Web
{
    internal class CsxJsInterop : JSModuleInterop
    {
        static CsxJsInterop _current;
        public static CsxJsInterop Current => _current;
        static CsxJsInterop()
        {
            _current = new CsxJsInterop();
            _current.ImportModule();
        }

        public CsxJsInterop() : base("CSX") { }
        
        static Action<WebEvent>? _handler;

        public void CreateElement(string tag, ulong id)
        {
            Console.WriteLine(nameof(CreateElement));

            Invoke(nameof(CreateElement), tag, id);

            //var callInfo = new JSCallInfo()
            //{
            //    FunctionIdentifier = "CSX." + nameof(CreateElement),
            //    ResultType = JSCallResultType.JSVoidResult,
            //    TargetInstanceId = 0,
            //};



            //InternalCalls.InvokeJS<string, string, string, string>(out var exception, ref callInfo, tag, id, null);

            //if (exception != null)
            //{
            //    Console.Error.WriteLine(exception);
            //    throw new Exception(exception);
            //}
        }

        public void RemoveElement(ulong id)
        {
            Console.WriteLine(nameof(RemoveElement));

            Invoke(nameof(RemoveElement), id);

            //var callInfo = new JSCallInfo()
            //{
            //    FunctionIdentifier = "CSX." + nameof(RemoveElement),
            //    ResultType = JSCallResultType.JSVoidResult,
            //    TargetInstanceId = 0,
            //};

            //InternalCalls.InvokeJS<string, string, string, string>(out var exception, ref callInfo, id, null, null);
            //if (exception != null)
            //{
            //    Console.Error.WriteLine(exception);
            //    throw new Exception(exception);
            //}
        }

        public void DestroyElement(ulong id)
        {
            Console.WriteLine(nameof(DestroyElement));

            Invoke(nameof(DestroyElement), id);

            //var callInfo = new JSCallInfo()
            //{
            //    FunctionIdentifier = "CSX." + nameof(DestroyElement),
            //    ResultType = JSCallResultType.JSVoidResult,
            //    TargetInstanceId = 0,
            //};

            //InternalCalls.InvokeJS<string, string, string, string>(out var exception, ref callInfo, id, null, null);
            //if (exception != null)
            //{
            //    Console.Error.WriteLine(exception);
            //    throw new Exception(exception);
            //}
        }


        public void SetElementAttribute(ulong id, string name, string value)
        {
            Console.WriteLine(nameof(SetElementAttribute));

            Invoke(nameof(SetElementAttribute), id, name, value);

            //var callInfo = new JSCallInfo()
            //{
            //    FunctionIdentifier = "CSX." + nameof(SetElementAttribute),
            //    ResultType = JSCallResultType.JSVoidResult,
            //    TargetInstanceId = 0,
            //};

            //InternalCalls.InvokeJS<string, string, string, string>(out var exception, ref callInfo, id, name, value);
            //if (exception != null)
            //{
            //    Console.Error.WriteLine(exception);
            //    throw new Exception(exception);
            //}
        }


        public void AttachElement(ulong parentId, ulong id)
        {
            Console.WriteLine(nameof(SetElementAttribute));

            Invoke(nameof(SetElementAttribute), parentId, id);

            //var callInfo = new JSCallInfo()
            //{
            //    FunctionIdentifier = "CSX." + nameof(AttachElement),
            //    ResultType = JSCallResultType.JSVoidResult,
            //    TargetInstanceId = 0,
            //};

            //InternalCalls.InvokeJS<string, string, string, string>(out var exception, ref callInfo, parentId, id, null);
            //if (exception != null)
            //{
            //    Console.Error.WriteLine(exception);
            //    throw new Exception(exception);
            //}
        }


        public void SetElementText(ulong id, string text)
        {
            Console.WriteLine(nameof(SetElementText));

            Invoke(nameof(SetElementText), id, text);

            //var callInfo = new JSCallInfo()
            //{
            //    FunctionIdentifier = "CSX." + nameof(SetElementText),
            //    ResultType = JSCallResultType.JSVoidResult,
            //    TargetInstanceId = 0,
            //};

            //InternalCalls.InvokeJS<string, string, string, string>(out var exception, ref callInfo, id, text, null);
            //if (exception != null)
            //{
            //    Console.Error.WriteLine(exception);
            //    throw new Exception(exception);
            //}
        }

        public void SetChildren(ulong id, string childrenCommaSeparated)
        {
            Console.WriteLine(nameof(SetElementText));

            Invoke(nameof(SetElementText), id, childrenCommaSeparated);

            //var callInfo = new JSCallInfo()
            //{
            //    FunctionIdentifier = "CSX." + nameof(SetChildren),
            //    ResultType = JSCallResultType.JSVoidResult,
            //    TargetInstanceId = 0,
            //};

            //InternalCalls.InvokeJS<string, string, string, string>(out var exception, ref callInfo, id, childrenCommaSeparated, null);
            //if (exception != null)
            //{
            //    Console.Error.WriteLine(exception);
            //    throw new Exception(exception);
            //}
        }

        public void SetElementAttributes(ulong id, string attrs)
        {
            Console.WriteLine(nameof(SetElementAttributes));

            Invoke(nameof(SetElementAttributes), id, attrs);

            //var callInfo = new JSCallInfo()
            //{
            //    FunctionIdentifier = "CSX." + nameof(SetElementAttributes),                
            //    ResultType = JSCallResultType.JSVoidResult,
            //    TargetInstanceId = 0,
            //};

            //InternalCalls.InvokeJS<string, string, string, string>(out var exception, ref callInfo, id, attrs, null);
            //if (exception != null)
            //{
            //    Console.Error.WriteLine(exception);
            //    throw new Exception(exception);
            //}
        }

        public static void SetEventHandler(Action<WebEvent> handler)
        {
            _handler = handler;
        }

        [JSInvokable]
        public static void OnEvent(WebEvent @event)
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

        public static void InvokeVoid<T, T1, T2>(string methodIdentifier, T arg, T1 arg1, T2 arg2)
        {
            var callInfo = new JSCallInfo()
            {
                FunctionIdentifier = methodIdentifier,
                ResultType = JSCallResultType.JSVoidResult,
                TargetInstanceId = 0,
            };

            InvokeJS<T, T1, T2, object>(out var exception, ref callInfo, arg, arg1, arg2);
            if (exception != null)
            {
                Console.Error.WriteLine("Error calling js {0}", methodIdentifier);
                Console.Error.WriteLine(exception);
                throw new Exception(exception);
            }
        }

        public static TR Invoke<TR, T, T1, T2>(string methodIdentifier, T arg, T1 arg1, T2 arg2)
        {
            JSCallResultType resultType;

            if (typeof(IJSUnmarshalledObjectReference).IsAssignableFrom(typeof(TR)))
            {
                resultType = JSCallResultType.JSObjectReference;
            }
            else if(typeof(IJSStreamReference).IsAssignableFrom(typeof(TR)))
            {
                resultType = JSCallResultType.JSStreamReference;
            }
            else
            {
                resultType = JSCallResultType.Default;
            }

            var callInfo = new JSCallInfo()
            {
                FunctionIdentifier = methodIdentifier,
                ResultType = resultType,
                TargetInstanceId = 0,
            };

            var result = InvokeJS<T, T1, T2, TR>(out var exception, ref callInfo, arg, arg1, arg2);
            if (exception != null)
            {
                Console.Error.WriteLine("Error calling js {0}", methodIdentifier);
                Console.Error.WriteLine(exception);
                throw new Exception(exception);
            }

            return result;
        }
            

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