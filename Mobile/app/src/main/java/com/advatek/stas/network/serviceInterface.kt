package com.advatek.stas.network

import com.advatek.stas.model.LoginRequest
import com.advatek.stas.model.LoginResponse
import com.advatek.stas.model.Scan
import com.advatek.stas.model.ScanIN
import com.advatek.stas.model.ScanOut
import retrofit2.Call
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.Header
import retrofit2.http.Headers
import retrofit2.http.POST
import retrofit2.http.Path

interface RestApi {

    @Headers("Content-Type: application/json")
    @POST("api/login")
    fun login(@Body request: LoginRequest): Call<LoginResponse>

    @POST("api/scan")
    fun addScan(@Header("Authorization") token: String, @Body scan: ScanIN): Call<ScanOut>

//    @GET("api/scan/getLastScanByEmployeeId/{employeeId}")
//    fun getLastScanByEmployeeId(@Path("employeeId") employeeId: Int): Call<Scan>
//
//    @GET("api/scan/check")
//    fun checkServer(): Call<Void>
}
