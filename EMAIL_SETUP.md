# 📧 Guía de Configuración de Email para TalentoPlus

## Opción 1: Gmail (Recomendado)

### Paso 1: Crear una Contraseña de Aplicación

1. Ve a tu cuenta de Google: https://myaccount.google.com/
2. En el menú izquierdo, selecciona **"Seguridad"**
3. En "Cómo inicias sesión en Google", activa la **Verificación en 2 pasos** (si no está activada)
4. Una vez activada, busca **"Contraseñas de aplicaciones"**
5. Selecciona:
   - **Aplicación**: Correo
   - **Dispositivo**: Otro (nombre personalizado) → escribe "TalentoPlus"
6. Haz clic en **"Generar"**
7. Copia la contraseña de 16 caracteres que aparece (sin espacios)

### Paso 2: Configurar en el Proyecto

#### Para ejecución local (appsettings.json):

Edita: `/TalentoPlus.Api/appsettings.json`

```json
{
  "Smtp": {
    "Host": "smtp.gmail.com",
    "Port": "587",
    "User": "tu-correo@gmail.com",
    "Password": "abcd efgh ijkl mnop"  // La contraseña de aplicación de 16 caracteres
  }
}
```

#### Para Docker (compose.yaml):

Edita: `/compose.yaml`

```yaml
talentoplus.api:
  environment:
    - Smtp__Host=smtp.gmail.com
    - Smtp__Port=587
    - Smtp__User=tu-correo@gmail.com
    - Smtp__Password=abcdefghijklmnop  # Sin espacios
```

---

## Opción 2: Outlook/Hotmail

```json
{
  "Smtp": {
    "Host": "smtp-mail.outlook.com",
    "Port": "587",
    "User": "tu-correo@outlook.com",
    "Password": "tu-contraseña"
  }
}
```

---

## Opción 3: Otros Proveedores

### Yahoo Mail
```json
{
  "Smtp": {
    "Host": "smtp.mail.yahoo.com",
    "Port": "587",
    "User": "tu-correo@yahoo.com",
    "Password": "tu-app-password"
  }
}
```

### Office 365
```json
{
  "Smtp": {
    "Host": "smtp.office365.com",
    "Port": "587",
    "User": "tu-correo@tuempresa.com",
    "Password": "tu-contraseña"
  }
}
```

---

## ✅ Verificar que Funciona

### Método 1: Usando Swagger UI

1. Inicia la API:
   ```bash
   dotnet run --project TalentoPlus.Api
   ```

2. Abre: `http://localhost:5001/swagger`

3. Prueba el endpoint `/api/empleados/autoregistro`:
   ```json
   {
     "firstName": "Test",
     "lastName": "User",
     "email": "tu-correo-de-prueba@gmail.com",
     "password": "Test123!",
     "jobTitle": "Tester",
     "departmentId": 1
   }
   ```

4. Si todo está bien, recibirás un email de bienvenida en tu bandeja de entrada.

### Método 2: Usando curl

```bash
curl -X POST "http://localhost:5001/api/empleados/autoregistro" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Test",
    "lastName": "User",
    "email": "tu-correo@gmail.com",
    "password": "Test123!",
    "jobTitle": "Developer",
    "departmentId": 1
  }'
```

---

## 🔧 Troubleshooting

### Error: "Authentication failed"
- ✅ Verifica que estés usando una **contraseña de aplicación**, no tu contraseña normal de Gmail
- ✅ Asegúrate de que la verificación en 2 pasos esté activada

### Error: "SMTP server requires a secure connection"
- ✅ Verifica que el puerto sea `587`
- ✅ El código ya usa `EnableSsl = true`

### No llega el email
- ✅ Revisa la carpeta de **Spam**
- ✅ Verifica que el email en el registro sea válido
- ✅ Revisa los logs de la API para ver errores

### Error: "SMTP not configured"
- ✅ Este mensaje aparece si las credenciales están vacías
- ✅ Es normal en desarrollo si no quieres configurar email
- ✅ El sistema seguirá funcionando, solo no enviará emails

---

## 📝 Ejemplo Completo de Configuración

**appsettings.json:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=TalentoDB;Username=postgres;Password=Qwe.123*"
  },
  "Jwt": {
    "Secret": "SuperSecretKey1234567890_ChangeMeInProd"
  },
  "Smtp": {
    "Host": "smtp.gmail.com",
    "Port": "587",
    "User": "talentoplus.rrhh@gmail.com",
    "Password": "abcd efgh ijkl mnop"
  }
}
```

**compose.yaml:**
```yaml
talentoplus.api:
  environment:
    - Smtp__Host=smtp.gmail.com
    - Smtp__Port=587
    - Smtp__User=talentoplus.rrhh@gmail.com
    - Smtp__Password=abcdefghijklmnop
```

---

## 🎯 Resumen Rápido

1. **Gmail**: Activa verificación en 2 pasos → Genera contraseña de aplicación
2. **Configura** en `appsettings.json` o `compose.yaml`
3. **Reinicia** la API
4. **Prueba** registrando un empleado
5. **Verifica** tu bandeja de entrada

¡Listo! 🚀
