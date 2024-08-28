using UnityEngine;

public class Internet : MonoBehaviour
{
    public static bool IsOkInternet()
    {
        switch (Application.internetReachability)
        {
            case NetworkReachability.NotReachable:
                Debug.Log("네트워크 연결 안됨");
                return false;
            case NetworkReachability.ReachableViaCarrierDataNetwork:
                Debug.Log("데이터로 연결됨");
                return true;
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                Debug.Log("Wifi나 케이블로 연결됨");
                return true;
        }

        return false;
    }
}