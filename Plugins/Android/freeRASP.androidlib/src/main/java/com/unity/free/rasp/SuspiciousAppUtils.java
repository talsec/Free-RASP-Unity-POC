package com.unity.free.rasp;

import com.aheaditec.talsec_security.security.api.SuspiciousAppInfo;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.List;

public class SuspiciousAppUtils {

    public static String SuspiciousAppToJSON(SuspiciousAppInfo suspiciousAppInfo) throws JSONException {
        JSONObject jsonObject = new JSONObject();
        jsonObject.put("package_name", suspiciousAppInfo.getPackageInfo().packageName);
        jsonObject.put("reason", suspiciousAppInfo.getReason());
        return jsonObject.toString();
    }

    public static String SuspiciousAppToJSON(List<SuspiciousAppInfo> suspiciousAppInfos) throws JSONException {
        JSONArray result = new JSONArray();
        for(int i=0; i<suspiciousAppInfos.size(); i++) {
            result.put(SuspiciousAppToJSON(suspiciousAppInfos.get(i)));
        }
        return result.toString();
    }
}
