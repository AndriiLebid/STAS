package com.advatek.stas.network

import com.advatek.stas.model.Scan
import com.advatek.stas.model.ScanOut
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class RestApiService {
    fun addScan(scanIn: Scan, onResult: (ScanOut?) -> Unit) {
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
}
