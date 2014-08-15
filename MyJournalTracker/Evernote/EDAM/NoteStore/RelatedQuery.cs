/**
 * Autogenerated by Thrift
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using Thrift.Protocol;
using Thrift.Transport;
namespace Evernote.EDAM.NoteStore
{

  #if !SILVERLIGHT && !NETFX_CORE
  [Serializable]
  #endif
  public partial class RelatedQuery : TBase
  {
    private string _noteGuid;
    private string _plainText;
    private NoteFilter _filter;
    private string _referenceUri;

    public string NoteGuid
    {
      get
      {
        return _noteGuid;
      }
      set
      {
        __isset.noteGuid = true;
        this._noteGuid = value;
      }
    }

    public string PlainText
    {
      get
      {
        return _plainText;
      }
      set
      {
        __isset.plainText = true;
        this._plainText = value;
      }
    }

    public NoteFilter Filter
    {
      get
      {
        return _filter;
      }
      set
      {
        __isset.filter = true;
        this._filter = value;
      }
    }

    public string ReferenceUri
    {
      get
      {
        return _referenceUri;
      }
      set
      {
        __isset.referenceUri = true;
        this._referenceUri = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT && !NETFX_CORE
    [Serializable]
    #endif
    public struct Isset {
      public bool noteGuid;
      public bool plainText;
      public bool filter;
      public bool referenceUri;
    }

    public RelatedQuery() {
    }

    public void Read (TProtocol iprot)
    {
      TField field;
      iprot.ReadStructBegin();
      while (true)
      {
        field = iprot.ReadFieldBegin();
        if (field.Type == TType.Stop) { 
          break;
        }
        switch (field.ID)
        {
          case 1:
            if (field.Type == TType.String) {
              NoteGuid = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              PlainText = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.Struct) {
              Filter = new NoteFilter();
              Filter.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.String) {
              ReferenceUri = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("RelatedQuery");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (NoteGuid != null && __isset.noteGuid) {
        field.Name = "noteGuid";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(NoteGuid);
        oprot.WriteFieldEnd();
      }
      if (PlainText != null && __isset.plainText) {
        field.Name = "plainText";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(PlainText);
        oprot.WriteFieldEnd();
      }
      if (Filter != null && __isset.filter) {
        field.Name = "filter";
        field.Type = TType.Struct;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        Filter.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (ReferenceUri != null && __isset.referenceUri) {
        field.Name = "referenceUri";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(ReferenceUri);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("RelatedQuery(");
      sb.Append("NoteGuid: ");
      sb.Append(NoteGuid);
      sb.Append(",PlainText: ");
      sb.Append(PlainText);
      sb.Append(",Filter: ");
      sb.Append(Filter== null ? "<null>" : Filter.ToString());
      sb.Append(",ReferenceUri: ");
      sb.Append(ReferenceUri);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
