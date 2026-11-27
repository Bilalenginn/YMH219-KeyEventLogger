# YMH219-KeyEventLogger

Windows Forms üzerinde C# ile geliştirilmiş, klavye olaylarının nasıl yakalandığını ve işlendiğini gösteren **eğitsel** bir tuş olayı kayıt uygulaması. Nesne Tabanlı Programlama (YMH219) dersi ek uygulama ödevi kapsamında hazırlanmıştır.

**Öğrenci:** Bilal Engin


## Kapsam ve Güvenlik Sınırı

Bu uygulama **gerçek bir sistem keyloggerı değildir.**

- Yalnızca **kendi penceresi aktifken** yapılan tuş girişlerini yakalar (formun `KeyDown` / `KeyUp` olayları).
- Global keyboard hook kullanılmaz.
- Arka planda veya gizli çalışmaz, başlangıçta otomatik açılmaz.
- Parola veya kimlik bilgisi yakalama, başka uygulamalardan veri toplama yapılmaz.
- Kayıtlar hiçbir koşulda ağ üzerinden gönderilmez; yalnızca kullanıcının isteğiyle yerel bir `.txt` dosyasına yazılır.
- Kayıt durumu arayüzde her zaman açıkça gösterilir (kırmızı "KAYIT AÇIK" / gri "KAYIT KAPALI" bandı).

## Kullanılan Teknolojiler

- C# / .NET 8
- Windows Forms (WinForms)
- Visual Studio 2022
- `StreamWriter` ile dosya işlemleri, LINQ ile istatistik
- Harici NuGet paketi kullanılmamıştır.

## Kurulum ve Çalıştırma

1. Depoyu klonlayın:
   ```
   git clone https://github.com/KULLANICI_ADI/YMH219-KeyEventLogger.git
   ```
2. `YMH219-KeyEventLogger.sln` dosyasını Visual Studio 2022 ile açın.
3. Gerekirse **.NET 8 SDK** ve "Masaüstü geliştirme (.NET)" iş yükünün yüklü olduğundan emin olun.
4. **F5** ile projeyi derleyip çalıştırın.

## Kullanım

| Buton | İşlev |
|---|---|
| **Başlat** | Kaydı açar. Üstteki bant kırmızı olur. |
| **Durdur** | Kaydı kapatır. Bant gri olur, tuşlar artık kaydedilmez. |
| **Temizle** | Ekrandaki kayıt listesini siler. |
| **Kaydet (.txt)** | Kayıtları seçilen metin dosyasına yazar. |

Kayıt açıkken uygulama penceresi aktifken basılan her tuş, tarih-saat bilgisiyle listeye eklenir. Örnek kayıt biçimi:

```
[2026-11-27 16:20:05.123]  KeyDown  A
[2026-11-27 16:20:05.201]  KeyUp    A
```

## Temel Özellikler

- `KeyDown` ve `KeyUp` olaylarının tarih-saat (milisaniye) ile listelenmesi
- Shift, Ctrl, Alt gibi değiştirici tuşlarla birlikte basılan kombinasyonların gösterimi (ör. `Control+C`)
- Başlat / Durdur ile kayıt durumunun kontrolü ve belirgin durum göstergesi
- Kayıtları temizleme
- Kayıtları `.txt` dosyasına kaydetme (hata durumunda kullanıcıya bildirim)
- Toplam tuş sayısı ve en sık kullanılan 3 tuşun istatistiği

## Proje Yapısı

| Dosya | Açıklama |
|---|---|
| `KeyLogEntry.cs` | Tek bir tuş olayını (zaman, tuş adı, olay türü) temsil eden sınıf |
| `KeyLogger.cs` | Kayıt durumu, kayıt listesi, dosyaya yazma ve istatistik mantığı |
| `Form1.cs` | Arayüz, olay yakalama (`KeyDown`/`KeyUp`) ve butonların işleyişi |

Kayıt mantığı (`KeyLogger`, `KeyLogEntry`) arayüzden bağımsız sınıflarda tutulmuştur; `Form1` yalnızca olayları bu sınıflara iletir ve sonucu gösterir.

## Ekran Görüntüleri

          ![Kayıt açık]
<img width="315" height="266" alt="key1" src="https://github.com/user-attachments/assets/652d2868-55fd-4503-9167-1a523725ccf2" />


           ![Kayıt kapalı]
<img width="317" height="269" alt="key2" src="https://github.com/user-attachments/assets/87061d57-7629-44a9-9c82-a2519d436eb9" />


## Bilinen Sınırlamalar / Geliştirme Notları

- Yalnızca uygulama penceresi aktifken çalışır; bu bilinçli bir tasarım kararıdır.
- Kayıtlar bellekte tutulur, uygulama kapanınca kaydedilmemişse kaybolur.
- Tarih/saat aralığına göre filtreleme eklenmemiştir.
