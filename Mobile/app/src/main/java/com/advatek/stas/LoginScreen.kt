package com.advatek.stas

import android.content.Context
import android.widget.Toast
import androidx.compose.foundation.layout.*
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.unit.dp
import com.advatek.stas.network.RestApiService
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.ui.text.input.KeyboardType

@Composable
fun LoginScreen(onLoginSuccess: () -> Unit) {
    var username by remember { mutableStateOf("") }
    var password by remember { mutableStateOf("") }
    val context = LocalContext.current

    Column(
        modifier = Modifier
            .fillMaxSize()
            .padding(16.dp),
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.Center
    ) {
        Text(text = "Login", style = MaterialTheme.typography.titleLarge)
        Spacer(modifier = Modifier.height(16.dp))
        OutlinedTextField(
            value = username,
            onValueChange = { username = it },
            label = { Text("Username") },
            modifier = Modifier.fillMaxWidth()
        )
        Spacer(modifier = Modifier.height(8.dp))
        OutlinedTextField(
            value = password,
            onValueChange = { password = it },
            label = { Text("Password") },
            modifier = Modifier.fillMaxWidth(),
            keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Password)
        )
        Spacer(modifier = Modifier.height(16.dp))
        Button(
            onClick = {
                handleLogin(context, username, password, onLoginSuccess)
            },
            modifier = Modifier.fillMaxWidth()
        ) {
            Text("Login")
        }
    }
}


fun handleLogin(context: Context, username: String, password: String, onLoginSuccess: () -> Unit) {
    val apiService = RestApiService(context)
    apiService.login(username, password) { response ->
        if (response?.token != null) {
            val sharedPreferences = context.getSharedPreferences("app_prefs", Context.MODE_PRIVATE)
            with(sharedPreferences.edit()) {
                putString("jwt_token", response.token)
                apply()
            }
            onLoginSuccess()
        } else {
            Toast.makeText(context, "Login failed", Toast.LENGTH_LONG).show()
        }
    }
}
