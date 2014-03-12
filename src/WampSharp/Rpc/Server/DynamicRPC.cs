using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WampSharp.Rpc.Server
{
	/// <summary>
	/// An implementation of <see cref="IWampRpcMetadata"/> and <see cref="IWampRpcMethod"/> in order to allow registration of dynamic RPCs.
	/// </summary>
	public class DynamicRPC: IWampRpcMetadata, IWampRpcMethod
	{
		private string mRPCID;
		private string mUri;
		private Func<object[], object> mMethod;
		private Type[] mParameterTypes;
		
		public DynamicRPC(string rpcID)
		{
			mRPCID = rpcID;
		}
		
		public IEnumerable<IWampRpcMethod> GetServiceMethods()
		{
			yield return this;
		}
		
		public void SetAction(Action action)
		{
			mMethod = (array) => {
				action();
				return null;
			};
			mParameterTypes = new Type[0];
		}
		
		public void SetAction<T1>(Action<T1> action)
		{
			mMethod = (array) => {
				action((T1)array[0]);
				return null;
			};
			mParameterTypes = new Type[]{typeof(T1)};
		}

		public void SetAction<T1, T2>(Action<T1, T2> action)
		{
			mMethod = (array) => {
				action((T1)array[0], (T2)array[1]);
				return null;
			};
			mParameterTypes = new Type[]{typeof(T1), typeof(T2)};
		}
		
		public void SetAction<T1, T2, T3>(Action<T1, T2, T3> action)
		{
			mMethod = (array) => {
				action((T1)array[0], (T2)array[1], (T3)array[2]);
				return null;
			};
			mParameterTypes = new Type[]{typeof(T1), typeof(T2), typeof(T3)};
		}
		
		public void SetAction<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
		{
			mMethod = (array) => {
				action((T1)array[0], (T2)array[1], (T3)array[2], (T4)array[3]);
				return null;
			};
			mParameterTypes = new Type[]{typeof(T1), typeof(T2), typeof(T3), typeof(T4)};
		}
		
		public void SetAction<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action)
		{
			mMethod = (array) => {
				action((T1)array[0], (T2)array[1], (T3)array[2], (T4)array[3], (T5)array[4]);
				return null;
			};
			mParameterTypes = new Type[]{typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)};
		}
		
		public string Name
		{
			get {return mRPCID;}
		}
		
		public string ProcUri
		{
			get {return mRPCID;}
		}
		
		public Type[] Parameters
		{
			get {return mParameterTypes;}
		}
		
		public System.Threading.Tasks.Task<object> InvokeAsync(object[] parameters)
		{
			return Task.Factory.StartNew(() => Invoke(parameters));
		}
		
		public object Invoke(object[] parameters)
		{
			return mMethod(parameters);
		}
	}
}