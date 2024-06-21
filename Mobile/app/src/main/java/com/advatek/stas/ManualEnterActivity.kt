package com.advatek.stas

import android.app.DatePickerDialog
import android.app.TimePickerDialog
import android.content.Context
import android.content.Intent
import android.os.Bundle
import android.widget.Toast
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.foundation.clickable
import androidx.compose.material3.Button
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.RadioButton
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.unit.dp
import com.advatek.stas.model.ScanIN
import com.advatek.stas.network.RestApiService
import com.advatek.stas.network.ServiceBuilder
import java.text.DateFormat
import java.text.SimpleDateFormat
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter
import java.util.Calendar
import java.util.Locale
import com.advatek.stas.network.connectionCheck
//import com.advatek.stas.network.isServerAvailable

class ManualEnterActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        val scanCode = intent.getStringExtra("scanCode")

        setContent {
            ManualEnter(scanCode = scanCode)
        }
    }
}

@Composable
fun ManualEnter(modifier: Modifier = Modifier, scanCode: String?) {

    val context = LocalContext.current
    val inButtonColors = ButtonDefaults.buttonColors(containerColor = Color.Blue)
    val outButtonColors = ButtonDefaults.buttonColors(containerColor = Color.Red)
    var selectedOption by remember { mutableStateOf("IN") }
    var selectedDate by remember { mutableStateOf("") }
    var selectedTime by remember { mutableStateOf("") }
    val radioOptions = listOf("IN", "OUT", "BREAKSTART", "BREAKEND", "LUNCHSTART", "LUNCHEND")

    val calendar = Calendar.getInstance()

    val datePickerDialog = DatePickerDialog(
        context,
        { _, year, month, dayOfMonth ->
            calendar.set(year, month, dayOfMonth)
            selectedDate = SimpleDateFormat("yyyy-MM-dd", Locale.getDefault()).format(calendar.time)
        },
        calendar.get(Calendar.YEAR),
        calendar.get(Calendar.MONTH),
        calendar.get(Calendar.DAY_OF_MONTH)
    )

    val timePickerDialog = TimePickerDialog(
        context,
        { _, hourOfDay, minute ->
            calendar.set(Calendar.HOUR_OF_DAY, hourOfDay)
            calendar.set(Calendar.MINUTE, minute)
            selectedTime = SimpleDateFormat("HH:mm", Locale.getDefault()).format(calendar.time)
        },
        calendar.get(Calendar.HOUR_OF_DAY),
        calendar.get(Calendar.MINUTE),
        true
    )

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
            Spacer(modifier = Modifier.height(64.dp))
            Text(text = "Scan Code: $scanCode", style = MaterialTheme.typography.titleLarge)
            Spacer(modifier = Modifier.height(32.dp))

            Row(
                horizontalArrangement = Arrangement.spacedBy(16.dp),
                modifier = Modifier.fillMaxWidth()
            ) {
                Button(
                    onClick = { datePickerDialog.show() },
                    modifier = Modifier.weight(1f),
                    shape = RoundedCornerShape(topEnd = 16.dp, bottomEnd = 16.dp)
                ) {
                    Text(text = "Select Date: $selectedDate")
                }

                Button(
                    onClick = { timePickerDialog.show() },
                    modifier = Modifier.weight(1f),
                    shape = RoundedCornerShape(topStart = 16.dp, bottomStart = 16.dp)
                ) {
                    Text(text = "Select Time: $selectedTime")
                }
            }

            Spacer(modifier = Modifier.height(8.dp))

            Box(
                modifier = Modifier.fillMaxWidth(),
                contentAlignment = Alignment.Center
            ) {
                Column(
                    horizontalAlignment = Alignment.CenterHorizontally
                ) {
                    for (i in radioOptions.indices step 2) {
                        Row(
                            horizontalArrangement = Arrangement.Center,
                            modifier = Modifier.fillMaxWidth()
                        ) {
                            RadioButton(
                                selected = selectedOption == radioOptions[i],
                                onClick = { selectedOption = radioOptions[i] }
                            )
                            Text(text = radioOptions[i], modifier = Modifier.clickable { selectedOption = radioOptions[i] })

                            if (i + 1 < radioOptions.size) {
                                Spacer(modifier = Modifier.width(16.dp))
                                RadioButton(
                                    selected = selectedOption == radioOptions[i + 1],
                                    onClick = { selectedOption = radioOptions[i + 1] }
                                )
                                Text(text = radioOptions[i + 1], modifier = Modifier.clickable { selectedOption = radioOptions[i + 1] })
                            }
                        }
                    }
                }
            }

            Spacer(modifier = Modifier.height(16.dp))

            Row(
                horizontalArrangement = Arrangement.spacedBy(16.dp),
                modifier = Modifier.fillMaxWidth()
            ) {
                Button(

                    onClick = {
                        val dateTime = LocalDateTime.parse("$selectedDate $selectedTime", DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm"))
                        handleAcceptAction(context,
                        ScanIN(
                            employeeCardNumber = scanCode ?: "",
                            scanDate = dateTime.format(DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss.SSS'Z'")),
                            scanType = selectedOption
                        )
                    )
                              },
                    modifier = Modifier.weight(1f),
                    colors = inButtonColors,
                    shape = RoundedCornerShape(topEnd = 16.dp, bottomEnd = 16.dp)
                ) {
                    Text("Accept")
                }
                Button(
                    onClick = { (context as ComponentActivity).finish() },
                    modifier = Modifier.weight(1f),
                    colors = outButtonColors,
                    shape = RoundedCornerShape(topStart = 16.dp, bottomStart = 16.dp)
                ) {
                    Text("Cancel")
                }
            }
        }
    }
}


fun handleAcceptAction(context: Context, scanIn: ScanIN) {

    if (!connectionCheck(context)) {
        Toast.makeText(context, "No internet connection available", Toast.LENGTH_LONG).show()

        (context as ComponentActivity).finish()
        return
    }


    val apiService = RestApiService(context)

    apiService.addScan(scanIn) {
        if (it?.scanId != null) {
            Toast.makeText(context, "Scan added successfully with ID: ${it.scanId}", Toast.LENGTH_LONG).show()
            //back to main here
            val intent = Intent(context, MainActivity::class.java)
            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP or Intent.FLAG_ACTIVITY_NEW_TASK)
            context.startActivity(intent)
        }
        else {
            Toast.makeText(context, "Validation Error happened, try again", Toast.LENGTH_LONG).show()
        }
    }
}