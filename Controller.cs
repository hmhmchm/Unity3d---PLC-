using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibplctagWrapper;
using System.Runtime.CompilerServices;
using System.Threading;
using System;
using UnityEngine.UI;

public class plccontroller : MonoBehaviour
{
    public ArrayList tagN = new ArrayList(); //어레이리스트 생성
    public int[] TestArray_Int;
    public float[] TestArray_Float;
    public bool[] TestArray_Bool;

    public int num;

    private const int DataTimeout = 5000;

    public Text ScriptTxt_I; // Int 1개짜리
    public Text ScriptTxt_IA; // Int 배열 텍스트 
    public Text ScriptTxt_F; // Float 1개짜리
    public Text ScriptTxt_FA; //Float 배열
    public Text ScriptTxt_B; // Bit 1개짜리
    public Text ScriptTxt_BA; //Bit 배열


    // Start is called before the first frame update
    void Start()
    {
        num = 0;
        //INT 배열
        TestArray_Int = new int[10];

        for(int i=0; i < 10 ; i++)
        {
            TestArray_Int[i] = i;
        }
        //Float 배열
        TestArray_Float = new float[10];

        for (int i = 0; i < 10; i++)
        {
            TestArray_Float[i] = i;
        }

        TestArray_Bool = new bool[32];

        for(int i = 0; i < 32; i++)
        {
            TestArray_Bool[i] = true;
        }

        



    }

    // Update is called once per frame
    void Update()
    {

        StartCoroutine("READ"); 
        StartCoroutine("WRITE");
        if(Input.GetKeyDown(KeyCode.S))
        {
            enabled = false;
        }
        Thread.Sleep(2000);
       

    }

    IEnumerator READ()
    {
        ReadTag_Int("READ_INT"); // INT  1개
        ReadTags_Int("READ_INTS", TestArray_Int.Length); // INT 배열
        ReadTag_Float("READ_FLOAT"); //FLOAT  1개
        ReadTags_Float("READ_FLOATS", TestArray_Float.Length); // FLOAT 배열
        ReadTag_Bit("READ_BIT"); // Bit 1개
        ReadTags_Bit("READ_BITS", 32); // Bit 배열
        yield return new WaitForSeconds(1f);
    }
    IEnumerator WRITE()
    {
        //WriteTag_Int("WRITE_INT", num); // num을 PLC 에 넣고싶다.
        WriteTags_Int("WRITE_INTS", TestArray_Int, TestArray_Int.Length);
       //WriteTags_Float("WRITE_FLOATS", TestArray_Float, TestArray_Float.Length); //Float 배열
       //
       //WriteTag_Bit("WRITE_BIT", true);
       WriteTags_Bit("WRITE_BITS", TestArray_Bool, TestArray_Bool.Length);
        yield return new WaitForSeconds(1f);
    }
    // ------------------------------------------ R  E  A  D --------------------------------------------
    // ========================================== R  E  A  D ============================================

    //Read Int 1개
    public void ReadTag_Int(string name)
    {
        var client = new Libplctag(); 
        var tag = new Tag("192.168.10.16", "1, 0", CpuType.LGX,name, DataType.Int32, 1, 0);
       
        client.AddTag(tag);

        while (client.GetStatus(tag) == Libplctag.PLCTAG_STATUS_PENDING)
        {
            Thread.Sleep(100);
        }
        if (client.GetStatus(tag) != Libplctag.PLCTAG_STATUS_OK)
        {
            Debug.Log($"Error setting up tag internal state. Error{ client.DecodeError(client.GetStatus(tag))}\n");
            return;
        }
        var result = client.ReadTag(tag, DataTimeout);
       
        if (result != Libplctag.PLCTAG_STATUS_OK)
        {
            Debug.Log($"ERROR: Unable to read the data! Got error code {result}: {client.DecodeError(result)}\n");
            return;
        }
        var TestArray = client.GetInt32Value(tag, 0*tag.ElementSize);

        ScriptTxt_I.text += "\n" + TestArray.ToString();

    }
    //Read Int 배열
     public void ReadTags_Int(string name, int length)
    {
        var client = new Libplctag();
        var tag = new Tag("192.168.10.16", "1, 0", CpuType.LGX, name, DataType.Int32, length);

        client.AddTag(tag);

        while (client.GetStatus(tag) == Libplctag.PLCTAG_STATUS_PENDING)
        {
            Thread.Sleep(100);
        }

        if (client.GetStatus(tag) != Libplctag.PLCTAG_STATUS_OK)
        {
            Debug.Log($"Error setting up tag internal state. Error{ client.DecodeError(client.GetStatus(tag))}\n");
            return;
        }
        var result = client.ReadTag(tag, DataTimeout);

        if (result != Libplctag.PLCTAG_STATUS_OK)
        {
            Debug.Log($"ERROR: Unable to read the data! Got error code {result}: {client.DecodeError(result)}\n");
            return;
        }
        
        for(int i = 0; i < length; i++)
        {
            TestArray_Int[i]  = client.GetInt32Value(tag, i * tag.ElementSize);
            ScriptTxt_IA.text += "\n"+TestArray_Int[i].ToString();
            //Debug.Log(TestArray_Int[i]);
        }

    }

    //Read Float 1개
    public void ReadTag_Float(string name)
    {
        var client = new Libplctag();
        var tag = new Tag("192.168.10.16", "1, 0", CpuType.LGX, name, DataType.Float32, 1, 0);

        client.AddTag(tag);

        while (client.GetStatus(tag) == Libplctag.PLCTAG_STATUS_PENDING)
        {
            Thread.Sleep(100);
        }
        if (client.GetStatus(tag) != Libplctag.PLCTAG_STATUS_OK)
        {
            Debug.Log($"Error setting up tag internal state. Error{ client.DecodeError(client.GetStatus(tag))}\n");
            return;
        }
        var result = client.ReadTag(tag, DataTimeout);

        if (result != Libplctag.PLCTAG_STATUS_OK)
        {
            Debug.Log($"ERROR: Unable to read the data! Got error code {result}: {client.DecodeError(result)}\n");
            return;
        }
        var TestArray = client.GetFloat32Value(tag, 0 * tag.ElementSize); //Test Array

        ScriptTxt_F.text += "\n" + TestArray.ToString();
    }
    
    //Read Float 배열
    public void ReadTags_Float(string name, int length)
    {
        var client = new Libplctag();
        var tag = new Tag("192.168.10.16", "1, 0", CpuType.LGX, name, DataType.Float32, length, 0);

        client.AddTag(tag);

        while (client.GetStatus(tag) == Libplctag.PLCTAG_STATUS_PENDING)
        {
            Thread.Sleep(100);
        }
        if (client.GetStatus(tag) != Libplctag.PLCTAG_STATUS_OK)
        {
            Debug.Log($"Error setting up tag internal state. Error{ client.DecodeError(client.GetStatus(tag))}\n");
            return;
        }
        var result = client.ReadTag(tag, DataTimeout);

        if (result != Libplctag.PLCTAG_STATUS_OK)
        {
            Debug.Log($"ERROR: Unable to read the data! Got error code {result}: {client.DecodeError(result)}\n");
            return;
        }
        for (int i = 0; i < length; i++)
        {
            TestArray_Float[i] = client.GetFloat32Value(tag, i * tag.ElementSize);
            ScriptTxt_FA.text += "\n" + TestArray_Float[i].ToString() + "\n";
            Debug.Log(TestArray_Float[i]);
        }
    }

    public void ReadTag_Bit(string name)
    {
        // Libplctag 를 쓰겟다.
        var client = new Libplctag();

        //tag에 PLC 입력하는 태그를 생성한다.
        var tag = new Tag("192.168.10.16", "1, 0", CpuType.LGX, name, DataType.Int32, 1);
        // client 인스턴스에 tag추가
        client.AddTag(tag);

        //태그 추가 후 GetStatus를 호출하여 상태 확인, 태그가 존재 할 시 GetStatus = 0
        while(client.GetStatus(tag) == Libplctag.PLCTAG_STATUS_PENDING)
        {
            Thread.Sleep(100);
        }

        //태그의 상태가 정상인지 확인
        if(client.GetStatus(tag) != Libplctag.PLCTAG_STATUS_OK)
        {
            Debug.Log("Error setting up tag internal state. Error{ client.DecodeError(client.GetStatus(tag))}\n");
            return;
        }

        //ReadTag 메소드를 사용하여 result에 반환
        var result = client.ReadTag(tag, DataTimeout);

        //result의 상태가 정상인지 확인
        if(result != Libplctag.PLCTAG_STATUS_OK)
        {
            Debug.Log("ERROR: Unable to read the data! Got error code {result}: {client.DecodeError(result)}\n");
            return;
        }

        //상태 확인 후 결과를 원하는 형식으로 변환
        var ReadTag = client.GetBitValue(tag, -1, DataTimeout);
        ScriptTxt_B.text = "\n" + ReadTag.ToString();

    }
    public void ReadTags_Bit(string name, int length)
    {
        var client = new Libplctag();
        //tag 에 PLC 정보입력
        var tag = new Tag("192.168.10.16", "1, 0", CpuType.LGX, name, DataType.Int32, 1);
        //client에 tag 추가
        client.AddTag(tag);

        //tag 추가 후 GetStatus를 호출하여 상태 확인, tag 존재 할 시 GetStatus = 0
        while (client.GetStatus(tag) == Libplctag.PLCTAG_STATUS_PENDING)
        {
            Thread.Sleep(100);
        }

        //tag의 상태가 정상인지 확인
        if(client.GetStatus(tag) != Libplctag.PLCTAG_STATUS_OK)
        {
            Debug.Log("Error setting up tag internal state. Error{ client.DecodeError(client.GetStatus(tag))}\n");
            return;
        }

        //ReadTag 메소드를 사용하여 result 에 반환
        var result = client.ReadTag(tag, DataTimeout);

        if(result != Libplctag.PLCTAG_STATUS_OK)
        {
            Debug.Log("ERROR: Unable to read the data! Got error code {result}: {client.DecodeError(result)}\n");
            return;
        }

        //상태 확인 후 결과를 변환시킴
        for(int i = 0; i < length; i++)
        {
            var ReadTag = client.GetBitValue(tag, i, DataTimeout);
            ScriptTxt_BA.text += "\n"+ ReadTag.ToString() + "\n";
        }
    }

    // =====================================W R I T E =======================================
    // =====================================W R I T E =======================================


    public void WriteTag_Int(string tagName, int num)
    {
        var client = new Libplctag();
        
        var tag = new Tag("192.168.10.16", "1, 0", CpuType.LGX, tagName, DataType.Int64, 1);

        client.AddTag(tag);
        
        while (client.GetStatus(tag) == Libplctag.PLCTAG_STATUS_PENDING)
        {
            Thread.Sleep(100);
        }
        
        if (client.GetStatus(tag) != Libplctag.PLCTAG_STATUS_OK)
        {
            Debug.Log("Error setting up tag internal state. Error{ plcClient.DecodeError(plcClient.GetStatus(tag))}\n");
            return;
        }
        
        client.SetInt32Value(tag, 0*tag.ElementSize, num); // write Data

        var result = client.WriteTag(tag, DataTimeout);

        if (result != Libplctag.PLCTAG_STATUS_OK)
        {
            Debug.Log("ERROR: Unable to read the data! Got error code Error{plcClient.DecodeError(result)}\n");
            return;
        }
        
    }
    public void WriteTags_Int(string tagName, int[] num, int length) 
    {
        
        var client = new Libplctag();

        var tag = new Tag("192.168.10.16", "1, 0", CpuType.LGX, tagName, DataType.Int32, length);

        client.AddTag(tag);

        while (client.GetStatus(tag) == Libplctag.PLCTAG_STATUS_PENDING)
        {
            Thread.Sleep(100);
        }

        if (client.GetStatus(tag) != Libplctag.PLCTAG_STATUS_OK)
        {
            Debug.Log("Error setting up tag internal state. Error{ plcClient.DecodeError(plcClient.GetStatus(tag))}\n");
            return;
        }
        for(int i = 0; i < length; i++)
        {
            client.SetInt32Value(tag, i * tag.ElementSize, num[i]); // write Data
        }

        var result = client.WriteTag(tag, DataTimeout);

        if (result != Libplctag.PLCTAG_STATUS_OK)
        {
            Debug.Log("ERROR: Unable to read the data! Got error code Error{plcClient.DecodeError(result)}\n");
            return;
        }
        
    }

    public void WriteTags_Float(string tagName, float[] num, int length)
    {
        //LibPlctag 가져와서 client 에 넣겠다.
        var client = new Libplctag();
        // tag에 태그 생성
        var tag = new Tag("192.168.10.16", "1, 0", CpuType.LGX, tagName, DataType.Float32, length);
        //client에 tag 추가
        client.AddTag(tag);
        //tag 추가 후에 GetStatus 를 호출하여 상태를 확인하고 태그가 존재 할 시에 GetStatus = 0
        while(client.GetStatus(tag) == Libplctag.PLCTAG_STATUS_PENDING)
        {
            Thread.Sleep(100); // 0.1초 동안 일시 중단.
        }

        // 태그가 정상인지 확인한다.
        if (client.GetStatus(tag) != Libplctag.PLCTAG_STATUS_OK)
        {
            Debug.Log("Error setting up tag internal state. Error{ plcClient.DecodeError(plcClient.GetStatus(tag))}\n");
            return;
        }

        //client 에 Int32형 num Set
        for(int i = 0; i <length; i++)
        {
            client.SetFloat32Value(tag, i * tag.ElementSize, num[i]); //write Data
        }

        var result = client.WriteTag(tag, DataTimeout);

        if(result != Libplctag.PLCTAG_STATUS_OK)
        {
            Debug.Log("ERROR: Unable to read the data! Got error code Error{plcClient.DecodeError(result)}\n");
            return;
        }
        
    }

    public void WriteTag_Bit(string name, bool boolValue)
    {
        var client = new Libplctag();
        //tag에 PLC 정보를 입력하여 태그 생성
        var tag = new Tag("192.168.10.16", "1, 0", CpuType.LGX, name, DataType.Int32, 1);

        //클라이언트 인스턴스에 tag추가
        client.AddTag(tag); 
        //태그 추가 후 GetStatus를 호출하여 상태 확인, 태그가 존재 할 시 GetStatus = 0
        
        while(client.GetStatus(tag) != Libplctag.PLCTAG_STATUS_OK)
        {
            Debug.Log("Error setting up tag internal state. Error{ client.DecodeError(client.GetStatus(tag))}\n");
            return;
        }

        // INT , FLOAT 와 BIT 다른점 
        client.SetBitValue(tag, -1, boolValue, DataTimeout);
        var result = client.WriteTag(tag, DataTimeout);

        if(result != Libplctag.PLCTAG_STATUS_OK)
        {
            Debug.Log("ERROR: Unable to read the data! Got error code {writeTag}: {client.DecodeError(writeTag)}\n");
            return;
        }
    }
    public void WriteTags_Bit(string name, bool[] boolValue, int length)
    {
        var client = new Libplctag();
        //tag에 PLC 정보를 입력하여 태그 생성
        var tag = new Tag("192.168.10.16", "1, 0", CpuType.LGX, name, DataType.Int32, 1);

        //클라이언트 인스턴스에 tag추가
        client.AddTag(tag);
        //태그 추가 후 GetStatus를 호출하여 상태 확인, 태그가 존재 할 시 GetStatus = 0

        while (client.GetStatus(tag) != Libplctag.PLCTAG_STATUS_OK)
        {
            Debug.Log("Error setting up tag internal state. Error{ client.DecodeError(client.GetStatus(tag))}\n");
            return;
        }

        // INT , FLOAT 와 BIT 다른점 
        for (int i = 0; i < length; i++)
        {
            client.SetBitValue(tag, i, boolValue[i], DataTimeout);
            var result = client.WriteTag(tag, DataTimeout);

            if (result != Libplctag.PLCTAG_STATUS_OK)
            {
                Debug.Log("ERROR: Unable to read the data! Got error code {writeTag}: {client.DecodeError(writeTag)}\n");
                return;
            }
        }
    }

}
