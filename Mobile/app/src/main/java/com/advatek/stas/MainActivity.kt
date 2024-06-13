package com.advatek.stas

import android.content.Context
import android.os.Bundle
import android.util.Log
import android.widget.TextClock
import android.widget.Toast
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.viewinterop.AndroidView
import com.advatek.stas.model.ScanIN
import com.advatek.stas.network.RestApiService
import com.advatek.stas.ui.theme.STASTheme
import java.time.LocalDateTime
import android.content.Intent

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            STASTheme {
                Scaffold(modifier = Modifier.fillMaxSize()) { innerPadding ->
                    ScanCodeForm(modifier = Modifier.padding(innerPadding))
                }
            }
        }
    }
}

@Composable
fun RealTimeClock() {
    AndroidView(
        factory = { context ->
            TextClock(context).apply {
                format12Hour = "hh:mm:ss a"
                textSize = 24f
            }
        },
        modifier = Modifier.padding(16.dp)
    )
}



@Composable
fun ScanCodeForm(modifier: Modifier = Modifier) {
    var scanCode by remember { mutableStateOf("") }
    val context = LocalContext.current

    val inButtonColors = ButtonDefaults.buttonColors(containerColor = Color.Blue)
    val outButtonColors = ButtonDefaults.buttonColors(containerColor = Color.Red)
    val grayButtonColors = ButtonDefaults.buttonColors(containerColor = Color.Gray)

    Box(
        modifier = modifier
            .fillMaxSize()
            .padding(16.dp),
        contentAlignment = Alignment.Center
    ) {
        Column(
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            RealTimeClock()
            Spacer(modifier = Modifier.height(96.dp))
            Text(text = "Scan Code", style = MaterialTheme.typography.titleLarge)
            Spacer(modifier = Modifier.height(8.dp))
            OutlinedTextField(
                value = scanCode,
                onValueChange = { scanCode = it },
                label = { Text("Enter code") },
                modifier = Modifier.fillMaxWidth()
            )
            Spacer(modifier = Modifier.height(16.dp))
            Row(
                horizontalArrangement = Arrangement.spacedBy(96.dp),
                modifier = Modifier.fillMaxWidth()
            ) {
                Column(
                    modifier = Modifier.weight(1f)
                ) {
                    Button(
                        onClick = { handleInAction(context, scanCode); scanCode = "" },
                        modifier = Modifier.fillMaxWidth(),
                        colors = inButtonColors,
                        shape = RoundedCornerShape(topEnd = 16.dp, bottomEnd = 16.dp)
                    ) {
                        Text("In")
                    }
                    Spacer(modifier = Modifier.height(8.dp))
                    Button(
                        onClick = { handleOutAction(context, scanCode); scanCode = "" },
                        modifier = Modifier.fillMaxWidth(),
                        colors = outButtonColors,
                        shape = RoundedCornerShape(topEnd = 16.dp, bottomEnd = 16.dp)
                    ) {
                        Text("Out")
                    }
                }
                Column(
                    modifier = Modifier.weight(1f)
                ) {
                    Button(
                        onClick = { handleBreakInAction(context, scanCode); scanCode = "" },
                        modifier = Modifier.fillMaxWidth(),
                        shape = RoundedCornerShape(topStart = 16.dp, bottomStart = 16.dp)
                    ) {
                        Text("Break Start")
                    }
                    Spacer(modifier = Modifier.height(8.dp))
                    Button(
                        onClick = { handleBreakOutAction(context, scanCode); scanCode = "" },
                        modifier = Modifier.fillMaxWidth(),
                        colors = grayButtonColors,
                        shape = RoundedCornerShape(topStart = 16.dp, bottomStart = 16.dp)

                    ) {
                        Text("Break End")
                    }
                    Spacer(modifier = Modifier.height(8.dp))
                    Button(
                        onClick = { handleLunchInAction(context, scanCode); scanCode = "" },
                        modifier = Modifier.fillMaxWidth(),
                        shape = RoundedCornerShape(topStart = 16.dp, bottomStart = 16.dp)
                    ) {
                        Text("Lunch Start")
                    }
                    Spacer(modifier = Modifier.height(8.dp))
                    Button(
                        onClick = { handleLunchOutAction(context, scanCode); scanCode = "" },
                        modifier = Modifier.fillMaxWidth(),
                        colors = grayButtonColors,
                        shape = RoundedCornerShape(topStart = 16.dp, bottomStart = 16.dp)
                    ) {
                        Text("Lunch End")
                    }
                }
            }
            Spacer(modifier = Modifier.height(48.dp))
            Row(
                horizontalArrangement = Arrangement.SpaceEvenly,
                modifier = Modifier.fillMaxWidth()
            ) {
                Button(onClick = { scanCode = "" }) {
                    Text("Cancel")
                }
            }
        }
    }
}


//fun handleAction(context: Context, scanCode: String, scanType: String) {
//    val apiService = RestApiService()
//
//    val scan = ScanIN(
//        employeeCardNumber = scanCode,
//        scanDate = LocalDateTime.now().toString(),
//        scanType = scanType
//    )
//
//    apiService.addScan(scan) {
//        if (it?.scanId != null) {
//            Toast.makeText(context, "Scan added successfully with ID: ${it.scanId}", Toast.LENGTH_LONG).show()
//        } else {
//
//            if(!scan.employeeCardNumber.isNullOrEmpty()){
//                val intent = Intent(context, ManualEnterActivity::class.java)
//                context.startActivity(intent)
//            }
//        }
//    }
//}

fun handleAction(context: Context, scanCode: String, scanType: String) {
    val apiService = RestApiService()

    val scan = ScanIN(
        employeeCardNumber = scanCode,
        scanDate = LocalDateTime.now().toString(),
        scanType = scanType
    )

    apiService.addScan(scan) {
        if (it?.scanId != null) {
            Toast.makeText(context, "Scan added successfully with ID: ${it.scanId}", Toast.LENGTH_LONG).show()
        } else {
            if (!scanCode.isNullOrEmpty()) {
                val intent = Intent(context, ManualEnterActivity::class.java)
                intent.putExtra("scanCode", scanCode)
                context.startActivity(intent)
            }
        }
    }
}


fun handleInAction(context: Context, scanCode: String) {
    handleAction(context, scanCode, "IN")
}

fun handleOutAction(context: Context, scanCode: String) {
    handleAction(context, scanCode, "OUT")
}

fun handleBreakInAction(context: Context, scanCode: String) {
    handleAction(context, scanCode, "BreakStart")
}
fun handleBreakOutAction(context: Context, scanCode: String) {
    handleAction(context, scanCode, "BreakEnd")
}
fun handleLunchInAction(context: Context, scanCode: String) {
    handleAction(context, scanCode, "LunchStart")
}
fun handleLunchOutAction(context: Context, scanCode: String) {
    handleAction(context, scanCode, "LunchEnd")
}


fun handleGetLastScan(employeeId: Int) {
    val apiService = RestApiService()
    Log.d("handleGetLastScan", "Requesting last scan for employeeId: $employeeId")
    apiService.getLastScanByEmployeeId(employeeId) { scan ->
        if (scan != null) {
            // Handle successful response
            Log.d("handleGetLastScan", "Received scan: $scan")
            println("Scan ID: ${scan.scanId}, Employee ID: ${scan.employeeId}")
        } else {
            // Handle error
            Log.e("handleGetLastScan", "Error retrieving scan")
            println("Error retrieving scan")
        }
    }
}


@Preview(showBackground = true)
@Composable
fun ScanCodeFormPreview() {
    STASTheme {
        ScanCodeForm()
    }
}

