package com.advatek.stas.network

import android.content.Context
import com.advatek.stas.model.LoginRequest
import com.advatek.stas.model.LoginResponse
import com.advatek.stas.model.ScanIN
import com.advatek.stas.model.ScanOut
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class RestApiService (private val context: Context) {

    private val retrofit = ServiceBuilder.buildService(RestApi::class.java)

    fun login(username: String, password: String, onResult: (LoginResponse?) -> Unit) {
        val call = retrofit.login(LoginRequest(username, password))
        call.enqueue(object : Callback<LoginResponse> {
            override fun onResponse(call: Call<LoginResponse>, response: Response<LoginResponse>) {
                onResult(response.body())
            }

            override fun onFailure(call: Call<LoginResponse>, t: Throwable) {
                onResult(null)
            }
        })
    }
    fun addScan(scan: ScanIN, onResult: (ScanOut?) -> Unit) {
        val sharedPreferences = context.getSharedPreferences("app_prefs", Context.MODE_PRIVATE)
        val token = sharedPreferences.getString("jwt_token", null)
        val call = retrofit.addScan("Bearer $token", scan)

        call.enqueue(object : Callback<ScanOut> {
            override fun onResponse(call: Call<ScanOut>, response: Response<ScanOut>) {
                onResult(response.body())
            }

            override fun onFailure(call: Call<ScanOut>, t: Throwable) {
                onResult(null)
            }
        })
    }



//    fun getLastScanByEmployeeId(employeeId: Int, onResult: (Scan?) -> Unit) {
//        val retrofit = ServiceBuilder.buildService(RestApi::class.java)
//        val call = retrofit.getLastScanByEmployeeId(employeeId)
//
//        Log.d("RestApiService", "Making network request to get last scan for employeeId: $employeeId")
//
//        call.enqueue(object : Callback<Scan> {
//            override fun onResponse(call: Call<Scan>, response: Response<Scan>) {
//                Log.d("RestApiService", "Received response: $response")
//                if (response.isSuccessful) {
//                    Log.d("RestApiService", "Response is successful")
//                    onResult(response.body())
//                } else {
//                    Log.e("RestApiService", "Error: ${response.errorBody()?.string()}")
//                    onResult(null)
//                }
//            }
//
//            override fun onFailure(call: Call<Scan>, t: Throwable) {
//                Log.e("RestApiService", "Failure: ${t.message}")
//                onResult(null)
//            }
//        })
//    }
}
