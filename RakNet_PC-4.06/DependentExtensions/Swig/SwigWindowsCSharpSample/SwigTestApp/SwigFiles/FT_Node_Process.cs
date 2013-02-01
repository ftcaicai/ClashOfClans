/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 2.0.9
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */

namespace RakNet {

using System;
using System.Runtime.InteropServices;

public class FT_Node_Process : IDisposable {
  private HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal FT_Node_Process(IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new HandleRef(this, cPtr);
  }

  internal static HandleRef getCPtr(FT_Node_Process obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  ~FT_Node_Process() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          RakNetPINVOKE.delete_FT_Node_Process(swigCPtr);
        }
        swigCPtr = new HandleRef(null, IntPtr.Zero);
      }
      GC.SuppressFinalize(this);
    }
  }

  public static FT_Node_Process GetInstance() {
    IntPtr cPtr = RakNetPINVOKE.FT_Node_Process_GetInstance();
    FT_Node_Process ret = (cPtr == IntPtr.Zero) ? null : new FT_Node_Process(cPtr, false);
    return ret;
  }

  public static void DestroyInstance(FT_Node_Process i) {
    RakNetPINVOKE.FT_Node_Process_DestroyInstance(FT_Node_Process.getCPtr(i));
  }

  public FT_Node_Process() : this(RakNetPINVOKE.new_FT_Node_Process(), true) {
    SwigDirectorConnect();
  }

  public virtual FT_MessageTypesNode GetNodeType() {
    FT_MessageTypesNode ret = (FT_MessageTypesNode)(SwigDerivedClassHasMethod("GetNodeType", swigMethodTypes0) ? RakNetPINVOKE.FT_Node_Process_GetNodeTypeSwigExplicitFT_Node_Process(swigCPtr) : RakNetPINVOKE.FT_Node_Process_GetNodeType(swigCPtr));
    return ret;
  }

  public virtual void OnProcess(FT_Session session, BitStream bsIn, AddressOrGUID systemIdentifier) {
    if (SwigDerivedClassHasMethod("OnProcess", swigMethodTypes1)) RakNetPINVOKE.FT_Node_Process_OnProcessSwigExplicitFT_Node_Process(swigCPtr, FT_Session.getCPtr(session), BitStream.getCPtr(bsIn), AddressOrGUID.getCPtr(systemIdentifier)); else RakNetPINVOKE.FT_Node_Process_OnProcess(swigCPtr, FT_Session.getCPtr(session), BitStream.getCPtr(bsIn), AddressOrGUID.getCPtr(systemIdentifier));
    if (RakNetPINVOKE.SWIGPendingException.Pending) throw RakNetPINVOKE.SWIGPendingException.Retrieve();
  }

  public virtual void OnOutTime(FT_Session session) {
    if (SwigDerivedClassHasMethod("OnOutTime", swigMethodTypes2)) RakNetPINVOKE.FT_Node_Process_OnOutTimeSwigExplicitFT_Node_Process(swigCPtr, FT_Session.getCPtr(session)); else RakNetPINVOKE.FT_Node_Process_OnOutTime(swigCPtr, FT_Session.getCPtr(session));
    if (RakNetPINVOKE.SWIGPendingException.Pending) throw RakNetPINVOKE.SWIGPendingException.Retrieve();
  }

  public void SetRakPeerInterface(RakPeerInterface ptr) {
    RakNetPINVOKE.FT_Node_Process_SetRakPeerInterface(swigCPtr, RakPeerInterface.getCPtr(ptr));
  }

  public void SetNodePlugin(FT_Node_Plugin ptr) {
    RakNetPINVOKE.FT_Node_Process_SetNodePlugin(swigCPtr, FT_Node_Plugin.getCPtr(ptr));
  }

  public void Send(FT_Session session, FT_DataBase data, AddressOrGUID systemIdentifier) {
    RakNetPINVOKE.FT_Node_Process_Send(swigCPtr, FT_Session.getCPtr(session), FT_DataBase.getCPtr(data), AddressOrGUID.getCPtr(systemIdentifier));
    if (RakNetPINVOKE.SWIGPendingException.Pending) throw RakNetPINVOKE.SWIGPendingException.Retrieve();
  }

  private void SwigDirectorConnect() {
    if (SwigDerivedClassHasMethod("GetNodeType", swigMethodTypes0))
      swigDelegate0 = new SwigDelegateFT_Node_Process_0(SwigDirectorGetNodeType);
    if (SwigDerivedClassHasMethod("OnProcess", swigMethodTypes1))
      swigDelegate1 = new SwigDelegateFT_Node_Process_1(SwigDirectorOnProcess);
    if (SwigDerivedClassHasMethod("OnOutTime", swigMethodTypes2))
      swigDelegate2 = new SwigDelegateFT_Node_Process_2(SwigDirectorOnOutTime);
    RakNetPINVOKE.FT_Node_Process_director_connect(swigCPtr, swigDelegate0, swigDelegate1, swigDelegate2);
  }

  private bool SwigDerivedClassHasMethod(string methodName, Type[] methodTypes) {
    System.Reflection.MethodInfo methodInfo = this.GetType().GetMethod(methodName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, methodTypes, null);
    bool hasDerivedMethod = methodInfo.DeclaringType.IsSubclassOf(typeof(FT_Node_Process));
    return hasDerivedMethod;
  }

  private int SwigDirectorGetNodeType() {
    return (int)GetNodeType();
  }

  private void SwigDirectorOnProcess(IntPtr session, IntPtr bsIn, IntPtr systemIdentifier) {
    OnProcess(new FT_Session(session, false), (bsIn == IntPtr.Zero) ? null : new BitStream(bsIn, false), new AddressOrGUID(systemIdentifier, false));
  }

  private void SwigDirectorOnOutTime(IntPtr session) {
    OnOutTime(new FT_Session(session, false));
  }

  public delegate int SwigDelegateFT_Node_Process_0();
  public delegate void SwigDelegateFT_Node_Process_1(IntPtr session, IntPtr bsIn, IntPtr systemIdentifier);
  public delegate void SwigDelegateFT_Node_Process_2(IntPtr session);

  private SwigDelegateFT_Node_Process_0 swigDelegate0;
  private SwigDelegateFT_Node_Process_1 swigDelegate1;
  private SwigDelegateFT_Node_Process_2 swigDelegate2;

  private static Type[] swigMethodTypes0 = new Type[] {  };
  private static Type[] swigMethodTypes1 = new Type[] { typeof(FT_Session), typeof(BitStream), typeof(AddressOrGUID) };
  private static Type[] swigMethodTypes2 = new Type[] { typeof(FT_Session) };
}

}
