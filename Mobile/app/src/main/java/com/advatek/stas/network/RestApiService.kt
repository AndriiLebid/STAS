package com.advatek.stas.network

import android.util.Log
import com.advatek.stas.model.Scan
import com.advatek.stas.model.ScanIN
import com.advatek.stas.model.ScanOut
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class RestApiService {
    fun addScan(scanIn: ScanIN, onResult: (ScanOut?) -> Unit) {
        val retrofit = ServiceBuilder.buildService(RestApi::class.java)
        retrofit.addScan(scanIn).enqueue(
            object : Callback<ScanOut> {
                override fun onFailure(call: Call<ScanOut>, t: Throwable) {
                    onResult(null)
                }

                override fun onResponse(call: Call<ScanOut>, response: Response<ScanOut>) {
                    val addScan = response.body()
                    onResult(addScan)
                }
            }
        )
    }

    fun getLastScanByEmployeeId(employeeId: Int, onResult: (Scan?) -> Unit) {
        val retrofit = ServiceBuilder.buildService(RestApi::class.java)
        val call = retrofit.getLastScanByEmployeeId(employeeId)

        Log.d("RestApiService", "Making network request to get last scan for employeeId: $employeeId")

        call.enqueue(object : Callback<Scan> {
            override fun onResponse(call: Call<Scan>, response: Response<Scan>) {
                Log.d("RestApiService", "Received response: $response")
                if (response.isSuccessful) {
                    Log.d("RestApiService", "Response is successful")
                    onResult(response.body())
                } else {
                    Log.e("RestApiService", "Error: ${response.errorBody()?.string()}")
                    onResult(null)
                }
            }

            override fun onFailure(call: Call<Scan>, t: Throwable) {
                Log.e("RestApiService", "Failure: ${t.message}")
                onResult(null)
            }
        })
    }
}
