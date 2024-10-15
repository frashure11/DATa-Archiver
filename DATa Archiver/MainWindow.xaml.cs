using System;
using System.IO;
using System.Media;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
namespace DATa_Archiver
{
        public partial class MainWindow : Window
        {
                internal List<int> listFileNames = new List<int>();
                internal List<int> listStartOffsets = new List<int>();
                internal List<int> listFileLengths = new List<int>();
                internal List<string> listActualFileNames = new List<string>();
                internal List<int> listFileNameOffsetsArchival = new List<int>();
                internal List<int> listFileDataOffsetsArchival = new List<int>();
                internal List<string> filePaths = new List<string>();
                internal string selectedFilePath;
                private Dictionary<string, string> currentLanguageDictionary;
                #region LanguageSupport
                private static readonly Dictionary<string, string> englishDictionary = new Dictionary<string, string>
                {
                        {"Can not add or remove files to an existing archive.", "Can not add or remove files to an existing archive." },
                        {"No changes to save.", "No changes to save." },
                        {"Error processing file. Lists are out of sync.", "Error processing file. Lists are out of sync." },
                        {"No file selected. Please select a file to unpack.", "No file selected. Please select a file to unpack." },
                        {"Invalid location value at row {i + 1}. Skipping file.", "Invalid location value at row {i + 1}. Skipping file." },
                        {"Error processing file: DataGrid is empty or not properly initialized.", "Error processing file: DataGrid is empty or not properly initialized." },
                        {"Error processing file. Index is out of range.", "Error processing file. Index is out of range." },
                        {"No file has been selected.", "No file has been selected." },
                        {"There are no files to save.", "There are no files to save." },
                        {"Sound file not found.", "Sound file not found." },
                        { "Location", "Location" },
                        { "View", "View" },
                        { "Language", "Language" },
                        { "Theme", "Theme" },
                        { "Light", "Light" },
                        { "Dark", "Dark" },
                        { "New", "New" },
                        { "File", "File" },
                        { "Open", "Open" },
                        { "Save", "Save" },
                        { "Save As", "Save As" },
                        { "Close File", "Close File" },
                        { "Exit", "Exit" },
                        { "Edit", "Edit" },
                        { "Add File", "Add File" },
                        { "Remove File", "Remove File" },
                        { "Unpack File", "Unpack File" },
                        { "Unpack All", "Unpack All" },
                        { "Preferences", "Preferences" },
                        { "No File Open", "No File Open" },
                        { "files found", " files found" },
                        { "Data Archiver", "DATa Archiver" },
                        { "Size", "Size" },
                        { "Save operation was canceled", "Save operation was canceled" },
                        { "No file has been selected for unpacking", "No file has been selected for unpacking" },
                        { "Save archive as", "Save archive as" },
                        { "Unsupported archive format. Header does not contain:", "Unsupported archive format. Header does not contain: " },
                        { "Error verifying archive", "Error verifying archive" },
                        { "Error processing file", "Error processing file" },
                        { "File has been successfully unpacked", "File has been successfully unpacked" },
                        { "No file has been selected. Please select a file to unpack.", "No file has been selected. Please select a file to unpack." },
                        { "All files in the archive have been successfully unpacked.", "All files in the archive have been successfully unpacked." },
                        { "Error unpacking files", "Error unpacking files" },
                        { "File too large!", "File too large!" },
                        { "No files selected", "No files selected" },
                        { "Archive saved successfully.", "Archive saved successfully." },
                        { "No files to save. Add files to create an archive.", "No files to save. Add files to create an archive." },
                        { "Error saving archive", "Error saving archive" },
                        { "Sound file not found", "Sound file not found" }
                };
                private static readonly Dictionary<string, string> spanishDictionary = new Dictionary<string, string>
                {
                        {"Can not add or remove files to an existing archive.", "No se pueden agregar ni eliminar archivos a un archivo existente." },
                        {"No changes to save.", "No hay cambios para guardar." },
                        {"Error processing file. Lists are out of sync.", "Error al procesar el archivo. Las listas están desincronizadas." },
                        {"No file selected. Please select a file to unpack.", "No se ha seleccionado ningún archivo. Por favor, seleccione un archivo para extraer." },
                        {"Invalid location value at row {i + 1}. Skipping file.", "Valor de ubicación no válido en la fila {i + 1}. Omitiendo archivo." },
                        {"Error processing file: DataGrid is empty or not properly initialized.", "Error al procesar el archivo: El DataGrid está vacío o no ha sido inicializado correctamente." },
                        {"Error processing file. Index is out of range.", "Error al procesar el archivo. El índice está fuera de rango." },
                        {"No file has been selected.", "No se ha seleccionado ningún archivo." },
                        {"There are no files to save.", "No hay archivos para guardar." },
                        {"Sound file not found.", "Archivo de sonido no encontrado." },
                        { "Location", "Ubicación" },
                        { "View", "Vista" },
                        { "Language", "Idioma" },
                        { "Theme", "Tema" },
                        { "Light", "Claro" },
                        { "Dark", "Oscuro" },
                        { "New", "Nuevo" },
                        { "File", "Archivo" },
                        { "Open", "Abrir" },
                        { "Save", "Guardar" },
                        { "Save As", "Guardar Como" },
                        { "Close File", "Cerrar Archivo" },
                        { "Exit", "Salir" },
                        { "Edit", "Editar" },
                        { "Add File", "Añadir Archivo" },
                        { "Remove File", "Eliminar Archivo" },
                        { "Unpack File", "Extraer Archivo" },
                        { "Unpack All", "Extraer Todos" },
                        { "Preferences", "Preferencias" },
                        { "No File Open", "Ningún Archivo Abierto" },
                        { "files found", " Archivos encontrados" },
                        { "Data Archiver", "Archivador de .dat" },
                        { "Size", "Tamaño" },
                        { "Save operation was canceled", "La operación de guardado fue cancelada" },
                        { "No file has been selected for unpacking", "No se ha seleccionado ningún archivo para extraer" },
                        { "Save archive as", "Guardar archivo como" },
                        { "Unsupported archive format. Header does not contain:", "Formato de archivo no compatible. El encabezado no contiene:" },
                        { "Error verifying archive", "Error al verificar el archivo" },
                        { "Error processing file", "Error al procesar el archivo" },
                        { "File has been successfully unpacked", "El archivo se ha extraído correctamente" },
                        { "No file has been selected. Please select a file to unpack.", "No se ha seleccionado ningún archivo. Por favor, seleccione un archivo para extraer." },
                        { "All files in the archive have been successfully unpacked.", "Todos los archivos del archivo se han extraído correctamente." },
                        { "Error unpacking files", "Error al extraer los archivos" },
                        { "File too large!", "¡El archivo es demasiado grande!" },
                        { "No files selected", "No se han seleccionado archivos" },
                        { "Archive saved successfully.", "Archivo guardado con éxito" },
                        { "No files to save. Add files to create an archive.", "No hay archivos para guardar. Añada archivos para crear un archivo." },
                        { "Error saving archive", "Error al guardar el archivo" },
                        { "Sound file not found", "Archivo de sonido no encontrado" }
                };
                private static readonly Dictionary<string, string> frenchDictionary = new Dictionary<string, string>
                {
                        {"Can not add or remove files to an existing archive.", "Impossible d'ajouter ou de supprimer des fichiers dans une archive existante." },
                        {"No changes to save.", "Aucun changement à enregistrer." },
                        {"Error processing file. Lists are out of sync.", "Erreur lors du traitement du fichier. Les listes sont désynchronisées." },
                        {"No file selected. Please select a file to unpack.", "Aucun fichier sélectionné. Veuillez sélectionner un fichier à extraire." },
                        {"Invalid location value at row {i + 1}. Skipping file.", "Valeur d'emplacement invalide à la ligne {i + 1}. Fichier ignoré." },
                        {"Error processing file: DataGrid is empty or not properly initialized.", "Erreur lors du traitement du fichier : la grille de données est vide ou mal initialisée." },
                        {"Error processing file. Index is out of range.", "Erreur lors du traitement du fichier. L'indice est hors limites." },
                        {"No file has been selected.", "Aucun fichier n'a été sélectionné." },
                        {"There are no files to save.", "Il n'y a aucun fichier à enregistrer." },
                        {"Sound file not found.", "Fichier audio introuvable." },
                        { "Location", "Emplacement" },
                        { "View", "Affichage" },
                        { "Language", "Langue" },
                        { "Theme", "Thème" },
                        { "Light", "Clair" },
                        { "Dark", "Sombre" },
                        { "New", "Nouveau" },
                        { "File", "Fichier" },
                        { "Open", "Ouvrir" },
                        { "Save", "Enregistrer" },
                        { "Save As", "Enregistrer Sous" },
                        { "Close File", "Fermer le Fichier" },
                        { "Exit", "Quitter" },
                        { "Edit", "Modifier" },
                        { "Add File", "Ajouter un Fichier" },
                        { "Remove File", "Supprimer le Fichier" },
                        { "Unpack File", "Extraire le Fichier" },
                        { "Unpack All", "Extraire Tous" },
                        { "Preferences", "Préférences" },
                        { "No File Open", "Aucun Fichier Ouvert" },
                        { "files found", " Fichiers trouvés" },
                        { "Data Archiver", "Archivage de .dat" },
                        { "Size", "Taille" },
                        { "Save operation was canceled", "L'opération d'enregistrement a été annulée" },
                        { "No file has been selected for unpacking", "Aucun fichier n'a été sélectionné pour l'extraction" },
                        { "Save archive as", "Enregistrer l'archive sous" },
                        { "Unsupported archive format. Header does not contain:", "Format d'archive non pris en charge. L'en-tête ne contient pas :" },
                        { "Error verifying archive", "Erreur lors de la vérification de l'archive" },
                        { "Error processing file", "Erreur lors du traitement du fichier" },
                        { "File has been successfully unpacked", "Le fichier a été extrait avec succès" },
                        { "No file has been selected. Please select a file to unpack.", "Aucun fichier n'a été sélectionné. Veuillez sélectionner un fichier à extraire." },
                        { "All files in the archive have been successfully unpacked.", "Tous les fichiers de l'archive ont été extraits avec succès." },
                        { "Error unpacking files", "Erreur lors de l'extraction des fichiers" },
                        { "File too large!", "Fichier trop volumineux !" },
                        { "No files selected", "Aucun fichier sélectionné" },
                        { "Archive saved successfully.", "Archive enregistrée avec succès" },
                        { "No files to save. Add files to create an archive.", "Aucun fichier à enregistrer. Ajoutez des fichiers pour créer une archive." },
                        { "Error saving archive", "Erreur lors de l'enregistrement de l'archive" },
                        { "Sound file not found", "Fichier audio introuvable" }
                };
                private static readonly Dictionary<string, string> germanDictionary = new Dictionary<string, string>
                {
                        {"Can not add or remove files to an existing archive.", "Dateien können nicht zu einem vorhandenen Archiv hinzugefügt oder daraus entfernt werden." },
                        {"No changes to save.", "Keine Änderungen zum Speichern." },
                        {"Error processing file. Lists are out of sync.", "Fehler bei der Dateiverarbeitung. Die Listen sind nicht synchron." },
                        {"No file selected. Please select a file to unpack.", "Keine Datei ausgewählt. Bitte wählen Sie eine Datei zum Entpacken aus." },
                        {"Invalid location value at row {i + 1}. Skipping file.", "Ungültiger Standortwert in Zeile {i + 1}. Datei wird übersprungen." },
                        {"Error processing file: DataGrid is empty or not properly initialized.", "Fehler bei der Dateiverarbeitung: Die Datenraster ist leer oder nicht richtig initialisiert." },
                        {"Error processing file. Index is out of range.", "Fehler bei der Dateiverarbeitung. Der Index ist außerhalb des gültigen Bereichs." },
                        {"No file has been selected.", "Es wurde keine Datei ausgewählt." },
                        {"There are no files to save.", "Es gibt keine Dateien zum Speichern." },
                        {"Sound file not found.", "Audiodatei nicht gefunden." },
                        { "Location", "Standort" },
                        { "View", "Ansicht" },
                        { "Language", "Sprache" },
                        { "Theme", "Thema" },
                        { "Light", "Hell" },
                        { "Dark", "Dunkel" },
                        { "New", "Neu" },
                        { "File", "Datei" },
                        { "Open", "Öffnen" },
                        { "Save", "Speichern" },
                        { "Save As", "Speichern Unter" },
                        { "Close File", "Datei Schließen" },
                        { "Exit", "Beenden" },
                        { "Edit", "Bearbeiten" },
                        { "Add File", "Datei Hinzufügen" },
                        { "Remove File", "Datei Entfernen" },
                        { "Unpack File", "Datei Entpacken" },
                        { "Unpack All", "Alles Entpacken" },
                        { "Preferences", "Einstellungen" },
                        { "No File Open", "Keine Datei Geöffnet" },
                        { "files found", " Dateien gefunden" },
                        { "Data Archiver", "Datenarchivierung" },
                        { "Size", "Größe" },
                        { "Save operation was canceled", "Der Speichervorgang wurde abgebrochen" },
                        { "No file has been selected for unpacking", "Keine Datei zum Entpacken ausgewählt" },
                        { "Save archive as", "Archiv speichern unter" },
                        { "Unsupported archive format. Header does not contain:", "Archivformat wird nicht unterstützt. Der Header enthält nicht:" },
                        { "Error verifying archive", "Fehler beim Überprüfen des Archivs" },
                        { "Error processing file", "Fehler beim Verarbeiten der Datei" },
                        { "File has been successfully unpacked", "Die Datei wurde erfolgreich entpackt" },
                        { "No file has been selected. Please select a file to unpack.", "Es wurde keine Datei ausgewählt. Bitte wählen Sie eine Datei zum Entpacken aus." },
                        { "All files in the archive have been successfully unpacked.", "Alle Dateien im Archiv wurden erfolgreich entpackt." },
                        { "Error unpacking files", "Fehler beim Entpacken der Dateien" },
                        { "File too large!", "Datei zu groß!" },
                        { "No files selected", "Keine Dateien ausgewählt" },
                        { "Archive saved successfully.", "Archiv erfolgreich gespeichert" },
                        { "No files to save. Add files to create an archive.", "Keine Dateien zum Speichern. Fügen Sie Dateien hinzu, um ein Archiv zu erstellen." },
                        { "Error saving archive", "Fehler beim Speichern des Archivs" },
                        { "Sound file not found", "Audiodatei nicht gefunden" }
                };
                private static readonly Dictionary<string, string> italianDictionary = new Dictionary<string, string>
                {
                        {"Can not add or remove files to an existing archive.", "Non è possibile aggiungere o rimuovere file da un archivio esistente." },
                        {"No changes to save.", "Nessuna modifica da salvare." },
                        {"Error processing file. Lists are out of sync.", "Errore durante l'elaborazione del file. Le liste non sono sincronizzate." },
                        {"No file selected. Please select a file to unpack.", "Nessun file selezionato. Si prega di selezionare un file da estrarre." },
                        {"Invalid location value at row {i + 1}. Skipping file.", "Valore di posizione non valido alla riga {i + 1}. File saltato." },
                        {"Error processing file: DataGrid is empty or not properly initialized.", "Errore durante l'elaborazione del file: DataGrid è vuoto o non correttamente inizializzato." },
                        {"Error processing file. Index is out of range.", "Errore durante l'elaborazione del file. L'indice è fuori dall'intervallo." },
                        {"No file has been selected.", "Nessun file è stato selezionato." },
                        {"There are no files to save.", "Non ci sono file da salvare." },
                        {"Sound file not found.", "File audio non trovato." },
                        { "Location", "Posizione" },
                        { "View", "Visualizza" },
                        { "Language", "Lingua" },
                        { "Theme", "Tema" },
                        { "Light", "Chiaro" },
                        { "Dark", "Scuro" },
                        { "New", "Nuovo" },
                        { "File", "File" },
                        { "Open", "Apri" },
                        { "Save", "Salva" },
                        { "Save As", "Salva Come" },
                        { "Close File", "Chiudi File" },
                        { "Exit", "Esci" },
                        { "Edit", "Modifica" },
                        { "Add File", "Aggiungi File" },
                        { "Remove File", "Rimuovi File" },
                        { "Unpack File", "Estrai File" },
                        { "Unpack All", "Estrai Tutto" },
                        { "Preferences", "Preferenze" },
                        { "No File Open", "Nessun File Aperto" },
                        { "files found", " File trovati" },
                        { "Data Archiver", "Archiviatore di .dat" },
                        { "Size", "Dimensione" },
                        { "Save operation was canceled", "Operazione di salvataggio annullata" },
                        { "No file has been selected for unpacking", "Nessun file selezionato per l'estrazione" },
                        { "Save archive as", "Salva archivio come" },
                        { "Unsupported archive format. Header does not contain:", "Formato archivio non supportato. L'intestazione non contiene:" },
                        { "Error verifying archive", "Errore durante la verifica dell'archivio" },
                        { "Error processing file", "Errore durante l'elaborazione del file" },
                        { "File has been successfully unpacked", "Il file è stato estratto con successo" },
                        { "No file has been selected. Please select a file to unpack.", "Nessun file selezionato. Si prega di selezionare un file da estrarre." },
                        { "All files in the archive have been successfully unpacked.", "Tutti i file nell'archivio sono stati estratti con successo." },
                        { "Error unpacking files", "Errore durante l'estrazione dei file" },
                        { "File too large!", "File troppo grande!" },
                        { "No files selected", "Nessun file selezionato" },
                        { "Archive saved successfully.", "Archivio salvato con successo" },
                        { "No files to save. Add files to create an archive.", "Nessun file da salvare. Aggiungi file per creare un archivio." },
                        { "Error saving archive", "Errore durante il salvataggio dell'archivio" },
                        { "Sound file not found", "File audio non trovato" }
                };
                private static readonly Dictionary<string, string> japaneseDictionary = new Dictionary<string, string>
                {
                        {"Can not add or remove files to an existing archive.", "既存のアーカイブにファイルを追加または削除できません。" },
                        {"No changes to save.", "保存する変更はありません。" },
                        {"Error processing file. Lists are out of sync.", "ファイル処理エラー。リストが同期されていません。" },
                        {"No file selected. Please select a file to unpack.", "ファイルが選択されていません。解凍するファイルを選択してください。" },
                        {"Invalid location value at row {i + 1}. Skipping file.", "行 {i + 1} の無効な位置値。ファイルをスキップします。" },
                        {"Error processing file: DataGrid is empty or not properly initialized.", "ファイル処理エラー: DataGridが空か、正しく初期化されていません。" },
                        {"Error processing file. Index is out of range.", "ファイル処理エラー。インデックスが範囲外です。" },
                        {"No file has been selected.", "ファイルが選択されていません。" },
                        {"There are no files to save.", "保存するファイルがありません。" },
                        {"Sound file not found.", "サウンドファイルが見つかりません。" },
                        { "Location", "場所" },
                        { "View", "表示" },
                        { "Language", "言語" },
                        { "Theme", "テーマ" },
                        { "Light", "ライト" },
                        { "Dark", "ダーク" },
                        { "New", "新規" },
                        { "File", "ファイル" },
                        { "Open", "開く" },
                        { "Save", "保存" },
                        { "Save As", "名前を付けて保存" },
                        { "Close File", "ファイルを閉じる" },
                        { "Exit", "終了" },
                        { "Edit", "編集" },
                        { "Add File", "ファイルを追加" },
                        { "Remove File", "ファイルを削除" },
                        { "Unpack File", "ファイルを解凍" },
                        { "Unpack All", "すべて解凍" },
                        { "Preferences", "設定" },
                        { "No File Open", "ファイルが開かれていません" },
                        { "files found", " ファイルが見つかりました" },
                        { "Data Archiver", "データアーカイバー" },
                        { "Size", "サイズ" },
                        { "Save operation was canceled", "保存操作がキャンセルされました" },
                        { "No file has been selected for unpacking", "解凍するファイルが選択されていません" },
                        { "Save archive as", "アーカイブを名前を付けて保存" },
                        { "Unsupported archive format. Header does not contain:", "サポートされていないアーカイブ形式です。ヘッダーに含まれていません:" },
                        { "Error verifying archive", "アーカイブの確認エラー" },
                        { "Error processing file", "ファイル処理エラー" },
                        { "File has been successfully unpacked", "ファイルが正常に解凍されました" },
                        { "No file has been selected. Please select a file to unpack.", "ファイルが選択されていません。解凍するファイルを選択してください。" },
                        { "All files in the archive have been successfully unpacked.", "アーカイブ内のすべてのファイルが正常に解凍されました。" },
                        { "Error unpacking files", "ファイルの解凍エラー" },
                        { "File too large!", "ファイルが大きすぎます！" },
                        { "No files selected", "ファイルが選択されていません" },
                        { "Archive saved successfully.", "アーカイブが正常に保存されました" },
                        { "No files to save. Add files to create an archive.", "保存するファイルがありません。アーカイブを作成するためにファイルを追加してください。" },
                        { "Error saving archive", "アーカイブの保存エラー" },
                        { "Sound file not found", "サウンドファイルが見つかりません" }
                };
                private static readonly Dictionary<string, string> chineseDictionary = new Dictionary<string, string>
                {
                        {"Can not add or remove files to an existing archive.", "无法向现有存档添加或删除文件。" },
                        {"No changes to save.", "没有要保存的更改。" },
                        {"Error processing file. Lists are out of sync.", "处理文件时出错。列表不同步。" },
                        {"No file selected. Please select a file to unpack.", "未选择文件。请选择要解压的文件。" },
                        {"Invalid location value at row {i + 1}. Skipping file.", "第 {i + 1} 行的位置信息无效。跳过文件。" },
                        {"Error processing file: DataGrid is empty or not properly initialized.", "处理文件时出错：DataGrid为空或未正确初始化。" },
                        {"Error processing file. Index is out of range.", "处理文件时出错。索引超出范围。" },
                        {"No file has been selected.", "未选择文件。" },
                        {"There are no files to save.", "没有要保存的文件。" },
                        {"Sound file not found.", "未找到声音文件。" },
                        { "Location", "位置" },
                        { "View", "视图" },
                        { "Language", "语言" },
                        { "Theme", "主题" },
                        { "Light", "浅色" },
                        { "Dark", "深色" },
                        { "New", "新建" },
                        { "File", "文件" },
                        { "Open", "打开" },
                        { "Save", "保存" },
                        { "Save As", "另存为" },
                        { "Close File", "关闭文件" },
                        { "Exit", "退出" },
                        { "Edit", "编辑" },
                        { "Add File", "添加文件" },
                        { "Remove File", "删除文件" },
                        { "Unpack File", "解压文件" },
                        { "Unpack All", "全部解压" },
                        { "Preferences", "首选项" },
                        { "No File Open", "未打开文件" },
                        { "files found", " 个文件已找到" },
                        { "Data Archiver", "数据归档器" },
                        { "Size", "大小" },
                        { "Save operation was canceled", "保存操作已取消" },
                        { "No file has been selected for unpacking", "未选择要解压的文件" },
                        { "Save archive as", "另存归档为" },
                        { "Unsupported archive format. Header does not contain:", "不支持的归档格式。标头不包含：" },
                        { "Error verifying archive", "验证归档时出错" },
                        { "Error processing file", "处理文件时出错" },
                        { "File has been successfully unpacked", "文件已成功解压" },
                        { "No file has been selected. Please select a file to unpack.", "未选择文件。请选择要解压的文件。" },
                        { "All files in the archive have been successfully unpacked.", "归档中的所有文件已成功解压。" },
                        { "Error unpacking files", "解压文件时出错" },
                        { "File too large!", "文件太大！" },
                        { "No files selected", "未选择文件" },
                        { "Archive saved successfully.", "归档已成功保存" },
                        { "No files to save. Add files to create an archive.", "没有要保存的文件。添加文件以创建归档。" },
                        { "Error saving archive", "保存归档时出错" },
                        { "Sound file not found", "未找到声音文件" }
                };
                private static readonly Dictionary<string, string> russianDictionary = new Dictionary<string, string>
                {
                        {"Can not add or remove files to an existing archive.", "Невозможно добавить или удалить файлы в существующем архиве." },
                        {"No changes to save.", "Нет изменений для сохранения." },
                        {"Error processing file. Lists are out of sync.", "Ошибка обработки файла. Списки не синхронизированы." },
                        {"No file selected. Please select a file to unpack.", "Файл не выбран. Пожалуйста, выберите файл для распаковки." },
                        {"Invalid location value at row {i + 1}. Skipping file.", "Недопустимое значение местоположения в строке {i + 1}. Пропуск файла." },
                        {"Error processing file: DataGrid is empty or not properly initialized.", "Ошибка обработки файла: DataGrid пуст или неправильно инициализирован." },
                        {"Error processing file. Index is out of range.", "Ошибка обработки файла. Индекс выходит за пределы допустимого диапазона." },
                        {"No file has been selected.", "Файл не выбран." },
                        {"There are no files to save.", "Нет файлов для сохранения." },
                        {"Sound file not found.", "Звуковой файл не найден." },
                        { "Location", "Местоположение" },
                        { "View", "Вид" },
                        { "Language", "Язык" },
                        { "Theme", "Тема" },
                        { "Light", "Светлая" },
                        { "Dark", "Тёмная" },
                        { "New", "Новый" },
                        { "File", "Файл" },
                        { "Open", "Открыть" },
                        { "Save", "Сохранить" },
                        { "Save As", "Сохранить как" },
                        { "Close File", "Закрыть файл" },
                        { "Exit", "Выйти" },
                        { "Edit", "Редактировать" },
                        { "Add File", "Добавить файл" },
                        { "Remove File", "Удалить файл" },
                        { "Unpack File", "Распаковать файл" },
                        { "Unpack All", "Распаковать все" },
                        { "Preferences", "Настройки" },
                        { "No File Open", "Файл не открыт" },
                        { "files found", " файлов найдено" },
                        { "Data Archiver", "Архиватор данных" },
                        { "Size", "Размер" },
                        { "Save operation was canceled", "Операция сохранения была отменена" },
                        { "No file has been selected for unpacking", "Файл для распаковки не выбран" },
                        { "Save archive as", "Сохранить архив как" },
                        { "Unsupported archive format. Header does not contain:", "Неподдерживаемый формат архива. Заголовок не содержит:" },
                        { "Error verifying archive", "Ошибка проверки архива" },
                        { "Error processing file", "Ошибка обработки файла" },
                        { "File has been successfully unpacked", "Файл успешно распакован" },
                        { "No file has been selected. Please select a file to unpack.", "Файл не выбран. Пожалуйста, выберите файл для распаковки." },
                        { "All files in the archive have been successfully unpacked.", "Все файлы в архиве успешно распакованы." },
                        { "Error unpacking files", "Ошибка при распаковке файлов" },
                        { "File too large!", "Файл слишком большой!" },
                        { "No files selected", "Файлы не выбраны" },
                        { "Archive saved successfully.", "Архив успешно сохранён" },
                        { "No files to save. Add files to create an archive.", "Нет файлов для сохранения. Добавьте файлы для создания архива." },
                        { "Error saving archive", "Ошибка сохранения архива" },
                        { "Sound file not found", "Звуковой файл не найден" }
                };
                private static readonly Dictionary<string, string> ukrainianDictionary = new Dictionary<string, string>
                {
                        {"Can not add or remove files to an existing archive.", "Неможливо додати або видалити файли в існуючому архіві." },
                        {"No changes to save.", "Немає змін для збереження." },
                        {"Error processing file. Lists are out of sync.", "Помилка обробки файлу. Списки не синхронізовані." },
                        {"No file selected. Please select a file to unpack.", "Файл не вибрано. Будь ласка, виберіть файл для розпакування." },
                        {"Invalid location value at row {i + 1}. Skipping file.", "Неприпустиме значення розташування в рядку {i + 1}. Пропускаємо файл." },
                        {"Error processing file: DataGrid is empty or not properly initialized.", "Помилка обробки файлу: DataGrid порожній або неправильно ініціалізований." },
                        {"Error processing file. Index is out of range.", "Помилка обробки файлу. Індекс виходить за межі допустимого діапазону." },
                        {"No file has been selected.", "Файл не вибрано." },
                        {"There are no files to save.", "Немає файлів для збереження." },
                        {"Sound file not found.", "Звуковий файл не знайдено." },
                        { "Location", "Місцезнаходження" },
                        { "View", "Вигляд" },
                        { "Language", "Мова" },
                        { "Theme", "Тема" },
                        { "Light", "Світла" },
                        { "Dark", "Темна" },
                        { "New", "Новий" },
                        { "File", "Файл" },
                        { "Open", "Відкрити" },
                        { "Save", "Зберегти" },
                        { "Save As", "Зберегти як" },
                        { "Close File", "Закрити файл" },
                        { "Exit", "Вийти" },
                        { "Edit", "Редагувати" },
                        { "Add File", "Додати файл" },
                        { "Remove File", "Видалити файл" },
                        { "Unpack File", "Розпакувати файл" },
                        { "Unpack All", "Розпакувати все" },
                        { "Preferences", "Налаштування" },
                        { "No File Open", "Файл не відкрито" },
                        { "files found", " файлів знайдено" },
                        { "Data Archiver", "Архіватор даних" },
                        { "Size", "Розмір" },
                        { "Save operation was canceled", "Операцію збереження скасовано" },
                        { "No file has been selected for unpacking", "Файл для розпакування не вибрано" },
                        { "Save archive as", "Зберегти архів як" },
                        { "Unsupported archive format. Header does not contain:", "Непідтримуваний формат архіву. Заголовок не містить:" },
                        { "Error verifying archive", "Помилка перевірки архіву" },
                        { "Error processing file", "Помилка обробки файлу" },
                        { "File has been successfully unpacked", "Файл успішно розпаковано" },
                        { "No file has been selected. Please select a file to unpack.", "Файл не вибрано. Будь ласка, виберіть файл для розпакування." },
                        { "All files in the archive have been successfully unpacked.", "Усі файли в архіві успішно розпаковано." },
                        { "Error unpacking files", "Помилка розпакування файлів" },
                        { "File too large!", "Файл занадто великий!" },
                        { "No files selected", "Файли не вибрано" },
                        { "Archive saved successfully.", "Архів успішно збережено" },
                        { "No files to save. Add files to create an archive.", "Немає файлів для збереження. Додайте файли для створення архіву." },
                        { "Error saving archive", "Помилка збереження архіву" },
                        { "Sound file not found", "Звуковий файл не знайдено" }
                };
                private static readonly Dictionary<string, string> polishDictionary = new Dictionary<string, string>
                {
                        {"Can not add or remove files to an existing archive.", "Nie można dodać ani usunąć plików z istniejącego archiwum." },
                        {"No changes to save.", "Brak zmian do zapisania." },
                        {"Error processing file. Lists are out of sync.", "Błąd przetwarzania pliku. Listy są niesynchronizowane." },
                        {"No file selected. Please select a file to unpack.", "Nie wybrano pliku. Proszę wybrać plik do rozpakowania." },
                        {"Invalid location value at row {i + 1}. Skipping file.", "Nieprawidłowa wartość lokalizacji w wierszu {i + 1}. Pomijanie pliku." },
                        {"Error processing file: DataGrid is empty or not properly initialized.", "Błąd przetwarzania pliku: DataGrid jest pusty lub nieprawidłowo zainicjowany." },
                        {"Error processing file. Index is out of range.", "Błąd przetwarzania pliku. Indeks poza zakresem." },
                        {"No file has been selected.", "Nie wybrano pliku." },
                        {"There are no files to save.", "Brak plików do zapisania." },
                        {"Sound file not found.", "Nie znaleziono pliku dźwiękowego." },
                        { "Location", "Lokalizacja" },
                        { "View", "Widok" },
                        { "Language", "Język" },
                        { "Theme", "Motyw" },
                        { "Light", "Jasny" },
                        { "Dark", "Ciemny" },
                        { "New", "Nowy" },
                        { "File", "Plik" },
                        { "Open", "Otwórz" },
                        { "Save", "Zapisz" },
                        { "Save As", "Zapisz jako" },
                        { "Close File", "Zamknij plik" },
                        { "Exit", "Zakończ" },
                        { "Edit", "Edytuj" },
                        { "Add File", "Dodaj plik" },
                        { "Remove File", "Usuń plik" },
                        { "Unpack File", "Rozpakuj plik" },
                        { "Unpack All", "Rozpakuj wszystko" },
                        { "Preferences", "Preferencje" },
                        { "No File Open", "Brak otwartego pliku" },
                        { "files found", " plików znaleziono" },
                        { "Data Archiver", "Archiwizator danych" },
                        { "Size", "Rozmiar" },
                        { "Save operation was canceled", "Operacja zapisu została anulowana" },
                        { "No file has been selected for unpacking", "Nie wybrano pliku do rozpakowania" },
                        { "Save archive as", "Zapisz archiwum jako" },
                        { "Unsupported archive format. Header does not contain:", "Nieobsługiwany format archiwum. Nagłówek nie zawiera:" },
                        { "Error verifying archive", "Błąd podczas weryfikacji archiwum" },
                        { "Error processing file", "Błąd przetwarzania pliku" },
                        { "File has been successfully unpacked", "Plik został pomyślnie rozpakowany" },
                        { "No file has been selected. Please select a file to unpack.", "Nie wybrano pliku. Proszę wybrać plik do rozpakowania." },
                        { "All files in the archive have been successfully unpacked.", "Wszystkie pliki w archiwum zostały pomyślnie rozpakowane." },
                        { "Error unpacking files", "Błąd podczas rozpakowywania plików" },
                        { "File too large!", "Plik jest za duży!" },
                        { "No files selected", "Nie wybrano plików" },
                        { "Archive saved successfully.", "Archiwum zostało pomyślnie zapisane" },
                        { "No files to save. Add files to create an archive.", "Brak plików do zapisania. Dodaj pliki, aby utworzyć archiwum." },
                        { "Error saving archive", "Błąd podczas zapisywania archiwum" },
                        { "Sound file not found", "Nie znaleziono pliku dźwiękowego" }
                };
                private static readonly Dictionary<string, string> koreanDictionary = new Dictionary<string, string>
                {
                        {"Can not add or remove files to an existing archive.", "기존 아카이브에 파일을 추가하거나 제거할 수 없습니다." },
                        {"No changes to save.", "저장할 변경 사항이 없습니다." },
                        {"Error processing file. Lists are out of sync.", "파일 처리 오류. 목록이 동기화되지 않았습니다." },
                        {"No file selected. Please select a file to unpack.", "파일이 선택되지 않았습니다. 압축을 풀 파일을 선택하십시오." },
                        {"Invalid location value at row {i + 1}. Skipping file.", "{i + 1}번째 행에서 잘못된 위치 값입니다. 파일을 건너뜁니다." },
                        {"Error processing file: DataGrid is empty or not properly initialized.", "파일 처리 오류: DataGrid가 비어 있거나 올바르게 초기화되지 않았습니다." },
                        {"Error processing file. Index is out of range.", "파일 처리 오류. 인덱스가 범위를 벗어났습니다." },
                        {"No file has been selected.", "선택된 파일이 없습니다." },
                        {"There are no files to save.", "저장할 파일이 없습니다." },
                        {"Sound file not found.", "사운드 파일을 찾을 수 없습니다." },
                        { "Location", "위치" },
                        { "View", "보기" },
                        { "Language", "언어" },
                        { "Theme", "테마" },
                        { "Light", "라이트" },
                        { "Dark", "다크" },
                        { "New", "새로 만들기" },
                        { "File", "파일" },
                        { "Open", "열기" },
                        { "Save", "저장" },
                        { "Save As", "다른 이름으로 저장" },
                        { "Close File", "파일 닫기" },
                        { "Exit", "종료" },
                        { "Edit", "편집" },
                        { "Add File", "파일 추가" },
                        { "Remove File", "파일 삭제" },
                        { "Unpack File", "파일 풀기" },
                        { "Unpack All", "모두 풀기" },
                        { "Preferences", "설정" },
                        { "No File Open", "파일이 열려 있지 않음" },
                        { "files found", " 파일을 찾았습니다" },
                        { "Data Archiver", "데이터 아카이버" },
                        { "Size", "크기" },
                        { "Save operation was canceled", "저장 작업이 취소되었습니다" },
                        { "No file has been selected for unpacking", "풀 파일이 선택되지 않았습니다" },
                        { "Save archive as", "아카이브를 다른 이름으로 저장" },
                        { "Unsupported archive format. Header does not contain:", "지원되지 않는 아카이브 형식입니다. 헤더에 포함되지 않음:" },
                        { "Error verifying archive", "아카이브 확인 오류" },
                        { "Error processing file", "파일 처리 오류" },
                        { "File has been successfully unpacked", "파일이 성공적으로 풀렸습니다" },
                        { "No file has been selected. Please select a file to unpack.", "파일이 선택되지 않았습니다. 풀 파일을 선택하십시오." },
                        { "All files in the archive have been successfully unpacked.", "아카이브의 모든 파일이 성공적으로 풀렸습니다." },
                        { "Error unpacking files", "파일 풀기 오류" },
                        { "File too large!", "파일이 너무 큽니다!" },
                        { "No files selected", "파일이 선택되지 않았습니다" },
                        { "Archive saved successfully.", "아카이브가 성공적으로 저장되었습니다" },
                        { "No files to save. Add files to create an archive.", "저장할 파일이 없습니다. 아카이브를 만들려면 파일을 추가하십시오." },
                        { "Error saving archive", "아카이브 저장 오류" },
                        { "Sound file not found", "사운드 파일을 찾을 수 없습니다" }
                };
                private static readonly Dictionary<string, string> portugueseDictionary = new Dictionary<string, string>
                {
                        {"Can not add or remove files to an existing archive.", "Não é possível adicionar ou remover arquivos de um arquivo existente." },
                        {"No changes to save.", "Não há alterações para salvar." },
                        {"Error processing file. Lists are out of sync.", "Erro ao processar o arquivo. As listas estão fora de sincronização." },
                        {"No file selected. Please select a file to unpack.", "Nenhum arquivo selecionado. Por favor, selecione um arquivo para descompactar." },
                        {"Invalid location value at row {i + 1}. Skipping file.", "Valor de localização inválido na linha {i + 1}. Pulando o arquivo." },
                        {"Error processing file: DataGrid is empty or not properly initialized.", "Erro ao processar o arquivo: o DataGrid está vazio ou não foi inicializado corretamente." },
                        {"Error processing file. Index is out of range.", "Erro ao processar o arquivo. O índice está fora do intervalo." },
                        {"No file has been selected.", "Nenhum arquivo foi selecionado." },
                        {"There are no files to save.", "Não há arquivos para salvar." },
                        {"Sound file not found.", "Arquivo de som não encontrado." },
                        { "Location", "Localização" },
                        { "View", "Exibir" },
                        { "Language", "Idioma" },
                        { "Theme", "Tema" },
                        { "Light", "Claro" },
                        { "Dark", "Escuro" },
                        { "New", "Novo" },
                        { "File", "Arquivo" },
                        { "Open", "Abrir" },
                        { "Save", "Salvar" },
                        { "Save As", "Salvar como" },
                        { "Close File", "Fechar arquivo" },
                        { "Exit", "Sair" },
                        { "Edit", "Editar" },
                        { "Add File", "Adicionar arquivo" },
                        { "Remove File", "Remover arquivo" },
                        { "Unpack File", "Descompactar arquivo" },
                        { "Unpack All", "Descompactar todos" },
                        { "Preferences", "Preferências" },
                        { "No File Open", "Nenhum arquivo aberto" },
                        { "files found", " arquivos encontrados" },
                        { "Data Archiver", "Arquivador de .dat" },
                        { "Size", "Tamanho" },
                        { "Save operation was canceled", "A operação de salvamento foi cancelada" },
                        { "No file has been selected for unpacking", "Nenhum arquivo foi selecionado para descompactação" },
                        { "Save archive as", "Salvar arquivo como" },
                        { "Unsupported archive format. Header does not contain:", "Formato de arquivo não suportado. O cabeçalho não contém:" },
                        { "Error verifying archive", "Erro ao verificar o arquivo" },
                        { "Error processing file", "Erro ao processar o arquivo" },
                        { "File has been successfully unpacked", "O arquivo foi descompactado com sucesso" },
                        { "No file has been selected. Please select a file to unpack.", "Nenhum arquivo foi selecionado. Por favor, selecione um arquivo para descompactar." },
                        { "All files in the archive have been successfully unpacked.", "Todos os arquivos no arquivo foram descompactados com sucesso." },
                        { "Error unpacking files", "Erro ao descompactar os arquivos" },
                        { "File too large!", "Arquivo muito grande!" },
                        { "No files selected", "Nenhum arquivo selecionado" },
                        { "Archive saved successfully.", "Arquivo salvo com sucesso" },
                        { "No files to save. Add files to create an archive.", "Não há arquivos para salvar. Adicione arquivos para criar um arquivo." },
                        { "Error saving archive", "Erro ao salvar o arquivo" },
                        { "Sound file not found", "Arquivo de som não encontrado" }
                };
                private static readonly Dictionary<string, string> dutchDictionary = new Dictionary<string, string>
                {
                        {"Can not add or remove files to an existing archive.", "Kan geen bestanden toevoegen of verwijderen aan een bestaand archief." },
                        {"No changes to save.", "Geen wijzigingen om op te slaan." },
                        {"Error processing file. Lists are out of sync.", "Fout bij het verwerken van bestand. Lijsten zijn niet gesynchroniseerd." },
                        {"No file selected. Please select a file to unpack.", "Geen bestand geselecteerd. Selecteer een bestand om uit te pakken." },
                        {"Invalid location value at row {i + 1}. Skipping file.", "Ongeldige locatiewaarde op rij {i + 1}. Bestand wordt overgeslagen." },
                        {"Error processing file: DataGrid is empty or not properly initialized.", "Fout bij het verwerken van bestand: DataGrid is leeg of niet correct geïnitialiseerd." },
                        {"Error processing file. Index is out of range.", "Fout bij het verwerken van bestand. Index is buiten bereik." },
                        {"No file has been selected.", "Er is geen bestand geselecteerd." },
                        {"There are no files to save.", "Er zijn geen bestanden om op te slaan." },
                        {"Sound file not found.", "Geluidsbestand niet gevonden." },
                        { "Location", "Locatie" },
                        { "View", "Beeld" },
                        { "Language", "Taal" },
                        { "Theme", "Thema" },
                        { "Light", "Licht" },
                        { "Dark", "Donker" },
                        { "New", "Nieuw" },
                        { "File", "Bestand" },
                        { "Open", "Openen" },
                        { "Save", "Opslaan" },
                        { "Save As", "Opslaan als" },
                        { "Close File", "Bestand sluiten" },
                        { "Exit", "Afsluiten" },
                        { "Edit", "Bewerken" },
                        { "Add File", "Bestand toevoegen" },
                        { "Remove File", "Bestand verwijderen" },
                        { "Unpack File", "Bestand uitpakken" },
                        { "Unpack All", "Alles uitpakken" },
                        { "Preferences", "Voorkeuren" },
                        { "No File Open", "Geen bestand geopend" },
                        { "files found", " bestanden gevonden" },
                        { "Data Archiver", "Gegevensarchiver" },
                        { "Size", "Grootte" },
                        { "Save operation was canceled", "De opslagbewerking is geannuleerd" },
                        { "No file has been selected for unpacking", "Er is geen bestand geselecteerd om uit te pakken" },
                        { "Save archive as", "Archief opslaan als" },
                        { "Unsupported archive format. Header does not contain:", "Niet-ondersteund archiefformaat. Header bevat niet:" },
                        { "Error verifying archive", "Fout bij het verifiëren van het archief" },
                        { "Error processing file", "Fout bij het verwerken van het bestand" },
                        { "File has been successfully unpacked", "Bestand succesvol uitgepakt" },
                        { "No file has been selected. Please select a file to unpack.", "Er is geen bestand geselecteerd. Selecteer een bestand om uit te pakken." },
                        { "All files in the archive have been successfully unpacked.", "Alle bestanden in het archief zijn succesvol uitgepakt." },
                        { "Error unpacking files", "Fout bij het uitpakken van bestanden" },
                        { "File too large!", "Bestand te groot!" },
                        { "No files selected", "Geen bestanden geselecteerd" },
                        { "Archive saved successfully.", "Archief succesvol opgeslagen" },
                        { "No files to save. Add files to create an archive.", "Geen bestanden om op te slaan. Voeg bestanden toe om een archief te maken." },
                        { "Error saving archive", "Fout bij het opslaan van het archief" },
                        { "Sound file not found", "Geluidsbestand niet gevonden" }
                };
                private static readonly Dictionary<string, string> greekDictionary = new Dictionary<string, string>
                {
                        {"Can not add or remove files to an existing archive.", "Δεν είναι δυνατή η προσθήκη ή η αφαίρεση αρχείων σε ένα υπάρχον αρχείο." },
                        {"No changes to save.", "Δεν υπάρχουν αλλαγές για αποθήκευση." },
                        {"Error processing file. Lists are out of sync.", "Σφάλμα κατά την επεξεργασία του αρχείου. Οι λίστες δεν είναι συγχρονισμένες." },
                        {"No file selected. Please select a file to unpack.", "Δεν έχει επιλεγεί αρχείο. Παρακαλώ επιλέξτε ένα αρχείο για αποσυμπίεση." },
                        {"Invalid location value at row {i + 1}. Skipping file.", "Μη έγκυρη τιμή τοποθεσίας στη σειρά {i + 1}. Παράκαμψη του αρχείου." },
                        {"Error processing file: DataGrid is empty or not properly initialized.", "Σφάλμα κατά την επεξεργασία του αρχείου: Το DataGrid είναι κενό ή δεν έχει αρχικοποιηθεί σωστά." },
                        {"Error processing file. Index is out of range.", "Σφάλμα κατά την επεξεργασία του αρχείου. Ο δείκτης είναι εκτός ορίων." },
                        {"No file has been selected.", "Δεν έχει επιλεγεί αρχείο." },
                        {"There are no files to save.", "Δεν υπάρχουν αρχεία για αποθήκευση." },
                        {"Sound file not found.", "Το αρχείο ήχου δεν βρέθηκε." },
                        { "Location", "Τοποθεσία" },
                        { "View", "Προβολή" },
                        { "Language", "Γλώσσα" },
                        { "Theme", "Θέμα" },
                        { "Light", "Φωτεινό" },
                        { "Dark", "Σκοτεινό" },
                        { "New", "Νέο" },
                        { "File", "Αρχείο" },
                        { "Open", "Άνοιγμα" },
                        { "Save", "Αποθήκευση" },
                        { "Save As", "Αποθήκευση ως" },
                        { "Close File", "Κλείσιμο αρχείου" },
                        { "Exit", "Έξοδος" },
                        { "Edit", "Επεξεργασία" },
                        { "Add File", "Προσθήκη αρχείου" },
                        { "Remove File", "Αφαίρεση αρχείου" },
                        { "Unpack File", "Αποσυμπίεση αρχείου" },
                        { "Unpack All", "Αποσυμπίεση όλων" },
                        { "Preferences", "Προτιμήσεις" },
                        { "No File Open", "Δεν υπάρχει ανοιχτό αρχείο" },
                        { "files found", " αρχεία βρέθηκαν" },
                        { "Data Archiver", "Αρχειοθέτης δεδομένων" },
                        { "Size", "Μέγεθος" },
                        { "Save operation was canceled", "Η αποθήκευση ακυρώθηκε" },
                        { "No file has been selected for unpacking", "Δεν επιλέχθηκε αρχείο για αποσυμπίεση" },
                        { "Save archive as", "Αποθήκευση αρχείου ως" },
                        { "Unsupported archive format. Header does not contain:", "Μη υποστηριζόμενη μορφή αρχείου. Η κεφαλίδα δεν περιέχει:" },
                        { "Error verifying archive", "Σφάλμα επαλήθευσης αρχείου" },
                        { "Error processing file", "Σφάλμα κατά την επεξεργασία αρχείου" },
                        { "File has been successfully unpacked", "Το αρχείο αποσυμπιέστηκε με επιτυχία" },
                        { "No file has been selected. Please select a file to unpack.", "Δεν επιλέχθηκε αρχείο. Παρακαλώ επιλέξτε ένα αρχείο για αποσυμπίεση." },
                        { "All files in the archive have been successfully unpacked.", "Όλα τα αρχεία αποσυμπιέστηκαν με επιτυχία." },
                        { "Error unpacking files", "Σφάλμα κατά την αποσυμπίεση των αρχείων" },
                        { "File too large!", "Το αρχείο είναι πολύ μεγάλο!" },
                        { "No files selected", "Δεν επιλέχθηκαν αρχεία" },
                        { "Archive saved successfully.", "Το αρχείο αποθηκεύτηκε με επιτυχία" },
                        { "No files to save. Add files to create an archive.", "Δεν υπάρχουν αρχεία για αποθήκευση. Προσθέστε αρχεία για να δημιουργήσετε ένα αρχείο." },
                        { "Error saving archive", "Σφάλμα κατά την αποθήκευση του αρχείου" },
                        { "Sound file not found", "Δεν βρέθηκε το αρχείο ήχου" }
                };
                private readonly Dictionary<SupportedLanguage, Dictionary<string, string>> languageDictionaries =
                new Dictionary<SupportedLanguage, Dictionary<string, string>>
                {
            { SupportedLanguage.English, englishDictionary },
            { SupportedLanguage.Español, spanishDictionary },
            { SupportedLanguage.Français, frenchDictionary },
            { SupportedLanguage.Deutsch, germanDictionary },
            { SupportedLanguage.Italiano, italianDictionary },
            { SupportedLanguage.日本語, japaneseDictionary },
            { SupportedLanguage.简体中文, chineseDictionary },
            { SupportedLanguage.Русский  , russianDictionary },
            { SupportedLanguage.Українська, ukrainianDictionary },
            { SupportedLanguage.Polski, polishDictionary },
            { SupportedLanguage.한국어, koreanDictionary },
            { SupportedLanguage.Português, portugueseDictionary },
            { SupportedLanguage.Nederlands, dutchDictionary },
            { SupportedLanguage.Ελληνικά, greekDictionary },
                };
                public void SwitchLanguage(SupportedLanguage language)
                {
                        //This rarely gets touched so leave plenty of comments to help preserve a good understanding

                        //Get the selected language dictionary
                        currentLanguageDictionary = languageDictionaries[language];

                        //Updating File menu and its sub-items
                        fileMenu.Header = currentLanguageDictionary["File"];
                        newMenuItem.Header = currentLanguageDictionary["New"];
                        openMenuItem.Header = currentLanguageDictionary["Open"];
                        saveMenuItem.Header = currentLanguageDictionary["Save"];
                        saveAsMenuItem.Header = currentLanguageDictionary["Save As"];
                        closeFileMenuItem.Header = currentLanguageDictionary["Close File"];
                        exitMenuItem.Header = currentLanguageDictionary["Exit"];

                        //Updating Edit menu and its sub-items
                        editMenu.Header = currentLanguageDictionary["Edit"];
                        addFileMenuItem.Header = currentLanguageDictionary["Add File"];
                        RemoveFileMenuItem.Header = currentLanguageDictionary["Remove File"];
                        unpackFileMenuItem.Header = currentLanguageDictionary["Unpack File"];
                        unpackAllMenuItem.Header = currentLanguageDictionary["Unpack All"];

                        //View menu and sub-items
                        viewMenu.Header = currentLanguageDictionary["View"];
                        //themeMenu.Header = currentLanguageDictionary["Theme"];
                        //lightThemeMenuItem.Header = currentLanguageDictionary["Light"];
                        //darkThemeMenuItem.Header = currentLanguageDictionary["Dark"];
                        languageMenu.Header = currentLanguageDictionary["Language"];

                        //Update DataGrid column headers
                        fileDataGrid.Columns[0].Header = currentLanguageDictionary["File"];
                        fileDataGrid.Columns[1].Header = currentLanguageDictionary["Size"];
                        fileDataGrid.Columns[2].Header = currentLanguageDictionary["Location"];

                        //Updating other UI elements if needed
                        Title = currentLanguageDictionary["Data Archiver"];
                        if (TempVars.NumberofFiles > 0)
                        {
                                label.Content = TempVars.NumberofFiles + currentLanguageDictionary["files found"];
                        }
                        else
                        {
                                label.Content = currentLanguageDictionary["No File Open"];
                        }
                }
                private void DetectSystemLanguage()
                {
                        var systemCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                        switch (systemCulture)
                        {
                                case "en":
                                        currentLanguageDictionary = englishDictionary;
                                        SwitchLanguage(SupportedLanguage.English);
                                        break;
                                case "es":
                                        currentLanguageDictionary = spanishDictionary;
                                        SwitchLanguage(SupportedLanguage.Español);
                                        break;
                                case "fr":
                                        currentLanguageDictionary = frenchDictionary;
                                        SwitchLanguage(SupportedLanguage.Français);
                                        break;
                                case "de":
                                        currentLanguageDictionary = germanDictionary;
                                        SwitchLanguage(SupportedLanguage.Deutsch);
                                        break;
                                case "it":
                                        currentLanguageDictionary = italianDictionary;
                                        SwitchLanguage(SupportedLanguage.Italiano);
                                        break;
                                case "ja":
                                        currentLanguageDictionary = japaneseDictionary;
                                        SwitchLanguage(SupportedLanguage.日本語);
                                        break;
                                case "zh":
                                        currentLanguageDictionary = chineseDictionary;
                                        SwitchLanguage(SupportedLanguage.简体中文);
                                        break;
                                case "ru":
                                        currentLanguageDictionary = russianDictionary;
                                        SwitchLanguage(SupportedLanguage.Русский);
                                        break;
                                case "uk":
                                        currentLanguageDictionary = ukrainianDictionary;
                                        SwitchLanguage(SupportedLanguage.Українська);
                                        break;
                                case "pl":
                                        currentLanguageDictionary = polishDictionary;
                                        SwitchLanguage(SupportedLanguage.Polski);
                                        break;
                                case "ko":
                                        currentLanguageDictionary = koreanDictionary;
                                        SwitchLanguage(SupportedLanguage.한국어);
                                        break;
                                case "pt":
                                        currentLanguageDictionary = portugueseDictionary;
                                        SwitchLanguage(SupportedLanguage.Português);
                                        break;
                                case "nl":
                                        currentLanguageDictionary = dutchDictionary;
                                        SwitchLanguage(SupportedLanguage.Nederlands);
                                        break;
                                case "el":
                                        currentLanguageDictionary = greekDictionary;
                                        SwitchLanguage(SupportedLanguage.Ελληνικά);
                                        break;
                                default:
                                        // If no match is found, set the default to English
                                        currentLanguageDictionary = englishDictionary;
                                        SwitchLanguage(SupportedLanguage.English);
                                        break;
                        }
                }
                private void PopulateLanguageMenu()
                {
                        languageMenu.Items.Clear(); // Clear existing items

                        foreach (SupportedLanguage language in Enum.GetValues(typeof(SupportedLanguage)))
                        {
                                MenuItem languageMenuItem = new MenuItem
                                {
                                        Header = language.ToString(), // Display language name
                                        Tag = language // Store enum value
                                };

                                // Handle the Click event to switch the language when clicked
                                languageMenuItem.Click += (s, e) =>
                                {
                                        MenuItem clickedItem = s as MenuItem;
                                        SupportedLanguage selectedLanguage = (SupportedLanguage)clickedItem.Tag;
                                        SwitchLanguage(selectedLanguage); // Call SwitchLanguage
            };

                                languageMenu.Items.Add(languageMenuItem); // Add to Language menu
                        }
                }
                #endregion
                #region ThemeSupport
                private void ApplySystemTheme()
                {
                        var systemTheme = GetSystemTheme(); // Get the system theme

                        var themeUri = systemTheme == SystemTheme.Dark
                            ? new Uri("DarkTheme.xaml", UriKind.Relative)
                            : new Uri("LightTheme.xaml", UriKind.Relative);

                        ResourceDictionary themeDict = new ResourceDictionary() { Source = themeUri };

                        Application.Current.Resources.MergedDictionaries.Clear();
                        Application.Current.Resources.MergedDictionaries.Add(themeDict);
                }
                public static SystemTheme GetSystemTheme()
                {
                        const string registryKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
                        const string registryValue = "AppsUseLightTheme";
                        var themeValue = Registry.GetValue(registryKey, registryValue, null);

                        if (themeValue != null && themeValue is int value)
                        {
                                return value == 0 ? SystemTheme.Dark : SystemTheme.Light;
                        }

                        // Default to light theme if unable to detect
                        return SystemTheme.Light;
                }
                private void SwitchToLightTheme()
                {
                        //var lightTheme = new Uri("LightTheme.xaml", UriKind.Relative);
                        //ResourceDictionary themeDict = new ResourceDictionary() { Source = lightTheme };

                        //Application.Current.Resources.MergedDictionaries.Clear();
                        //Application.Current.Resources.MergedDictionaries.Add(themeDict);
                }
                private void SwitchToDarkTheme()
                {
                        //var darkTheme = new Uri("DarkTheme.xaml", UriKind.Relative);
                        //ResourceDictionary themeDict = new ResourceDictionary() { Source = darkTheme };

                        //Application.Current.Resources.MergedDictionaries.Clear();
                        //Application.Current.Resources.MergedDictionaries.Add(themeDict);
                }
                #endregion
                public MainWindow()
                {
                        InitializeComponent();
                        CheckAndSetDebugMode();
                        ApplySystemTheme();
                        DetectSystemLanguage();
                        PopulateLanguageMenu();
                }
                private void CheckAndSetDebugMode()
                {
                        if (App.DebugFlag)
                        {
                                TempVars.DebugMode = true;
                                MessageBox.Show("Debug Mode is Enabled", "Debug", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                                TempVars.DebugMode = false;
                        }
                }
                private void NewFile_Click(object sender, RoutedEventArgs e)
                {
                        ClearData();
                }
                private void OpenFile_Click(object sender, RoutedEventArgs e)
                {
                        ClearData();
                        OpenFile();
                }
                private void SaveFile_Click(object sender, RoutedEventArgs e)
                {
                        if (TempVars.ExistingFile == true && !string.IsNullOrEmpty(TempVars.fileName))
                        {
                                if (TempVars.ChangesHaveBeenMade == true)
                                {
                                        SaveArchive(TempVars.fileName);
                                        TempVars.ChangesHaveBeenMade = false;
                                }
                                else
                                {
                                        if (TempVars.DebugMode)
                                        {
                                                System.Windows.MessageBox.Show(currentLanguageDictionary["No changes to save."]);
                                        }
                                        PlayWindowsSound("Windows Print complete.wav");
                                }
                        }
                        else
                        {
                                // If no existing file or filename, fallback to Save As...
                                SaveFileAs_Click(sender, e);
                        }
                }
                private void SaveFileAs_Click(object sender, RoutedEventArgs e)
                {
                        SaveFileDialog saveFileDialog = new SaveFileDialog
                        {
                                Filter = "DAT files (*.dat)|*.dat",
                                DefaultExt = "dat",
                                AddExtension = true,
                                Title = currentLanguageDictionary["Save archive as"]

                        };
                        if (saveFileDialog.ShowDialog() == true)
                        {
                                SaveArchive(saveFileDialog.FileName);
                                TempVars.fileName = saveFileDialog.FileName;
                                TempVars.ExistingFile = true;
                        }
                        else
                        {
                                System.Windows.MessageBox.Show(currentLanguageDictionary["Save operation was canceled"]);

                        }
                }
                private void CloseFile_Click(object sender, RoutedEventArgs e)
                {
                        ClearData();
                }
                private void Exit_Click(object sender, RoutedEventArgs e)
                {
                        this.Close();
                }
                private void Unpack_Single_Click(object sender, RoutedEventArgs e)
                {
                        if (!string.IsNullOrEmpty(selectedFilePath))
                        {
                                Unpack_Single(selectedFilePath);
                        }
                        else
                        {
                                PlayWindowsSound("Windows Error.wav");
                                if (TempVars.DebugMode)
                                {
                                        System.Windows.MessageBox.Show(currentLanguageDictionary["No file has been selected for unpacking."]);
                                }
                        }
                }
                private void Unpack_All_Click(object sender, RoutedEventArgs e)
                {
                        if (!string.IsNullOrEmpty(selectedFilePath))
                        {
                                Unpack_All(selectedFilePath);
                        }
                        else
                        {
                                PlayWindowsSound("Windows Error.wav");
                                if (TempVars.DebugMode)
                                {
                                        System.Windows.MessageBox.Show(currentLanguageDictionary["No file has been selected for unpacking."]);
                                }
                        }
                }
                private void ImportFile_Click(object sender, RoutedEventArgs e)
                {
                        if(TempVars.ExistingFile)
                        {
                                System.Windows.MessageBox.Show(currentLanguageDictionary["Can not add or remove files to an existing archive."]);
                        }
                        else
                        {
                                ImportFile();
                        }
                }
                private void RemoveFile_Click(object sender, RoutedEventArgs e)
                {
                        if (TempVars.ExistingFile)
                        {
                                System.Windows.MessageBox.Show(currentLanguageDictionary["Can not add or remove files to an existing archive."]);
                        }
                        else
                        {
                                RemoveSelectedFile();
                        }
                }
                private void Theme_Light_Click(object sender, RoutedEventArgs e)
                {
                        SwitchToLightTheme();
                }
                private void Theme_Dark_Click(object sender, RoutedEventArgs e)
                {
                        SwitchToDarkTheme();
                }
                private void ClearData()
                {
                        fileDataGrid.ItemsSource = null;
                        listFileNames.Clear();
                        listStartOffsets.Clear();
                        listFileLengths.Clear();
                        listActualFileNames.Clear();
                        TempVars.NumberofFiles = 0;
                        selectedFilePath = string.Empty;
                        TempVars.ExistingFile = false;
                        TempVars.fileName = "";
                        TempVars.ChangesHaveBeenMade = false;
                        label.Content = currentLanguageDictionary["No File Open"];
                }
                internal void OpenFile()
                {
                        OpenFileDialog openFileDialog = new OpenFileDialog
                        {
                                Filter = "DAT files (*.dat)|*.dat|All files (*.*)|*.*"
                        };
                        if (openFileDialog.ShowDialog() == true)
                        {
                                //Do not put ClearData() in here, it will interfere due to it clearing selectedFilePath. Fields must be manually cleared here.
                                fileDataGrid.ItemsSource = null;
                                listFileNames.Clear();
                                listStartOffsets.Clear();
                                listFileLengths.Clear();
                                listActualFileNames.Clear();
                                TempVars.NumberofFiles = 0;
                                TempVars.ExistingFile = false;
                                TempVars.fileName = "";
                                TempVars.ChangesHaveBeenMade = false;
                                label.Content = currentLanguageDictionary["No File Open"];
                                selectedFilePath = openFileDialog.FileName;
                                if (VerifyArchiveSupport(selectedFilePath))
                                {
                                        ProcessDAT(selectedFilePath);
                                        TempVars.fileName = selectedFilePath;
                                        TempVars.ExistingFile = true;
                                        TempVars.ChangesHaveBeenMade = false;
                                }
                                else
                                {
                                        //Potentially allow an override here?
                                        PlayWindowsSound("Windows Error.wav");
                                        System.Windows.MessageBox.Show(currentLanguageDictionary["Unsupported archive format. Header does not contain:"] + "'archive  V2.DMZ'");

                                }
                        }
                        TempVars.ExistingFile = true;
                }
                private bool VerifyArchiveSupport(string filePath)
                {
                        byte[] expectedHeader = new byte[]
                        {
                0x61, 0x72, 0x63, 0x68, 0x69, 0x76, 0x65, 0x20,
                0x20, 0x56, 0x32, 0x2E, 0x44, 0x4D, 0x5A, 0x00
                        };
                        try
                        {
                                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                                {
                                        byte[] fileHeader = new byte[16];
                                        fs.Read(fileHeader, 0, fileHeader.Length);

                                        for (int i = 0; i < expectedHeader.Length; i++)
                                        {
                                                if (fileHeader[i] != expectedHeader[i])
                                                {
                                                        return false; // Header does not match
                                                }
                                        }
                                }
                                return true; // Header matches
                        }
                        catch (Exception ex)
                        {
                                PlayWindowsSound("Windows Error.wav");
                                System.Windows.MessageBox.Show($"{currentLanguageDictionary["Error verifying archive"]} {ex.Message}");
                                return false;
                        }
                }
                private void ProcessDAT(string filePath)
                {
                        try
                        {
                                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                                {
                                        using (BinaryReader reader = new BinaryReader(fs))
                                        {
                                                fs.Seek(16, SeekOrigin.Begin);
                                                TempVars.NumberofFiles = reader.ReadInt32();
                                                for (int i = 0; i < TempVars.NumberofFiles; i++)
                                                {
                                                        listFileNames.Add(reader.ReadInt32());
                                                        listStartOffsets.Add(reader.ReadInt32());
                                                        listFileLengths.Add(reader.ReadInt32());
                                                }
                                        }
                                }
                                FileNameGrabber(filePath);
                                PopulateDataGrid();
                                label.Content = TempVars.NumberofFiles + currentLanguageDictionary["files found"];

                        }
                        catch (Exception ex)
                        {
                                PlayWindowsSound("Windows Error.wav");
                                System.Windows.MessageBox.Show($"Error processing DAT file: {ex.Message}");
                        }
                }
                private void FileNameGrabber(string filePath)
                {
                        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                                using (BinaryReader reader = new BinaryReader(fs))
                                {
                                        for (int i = 0; i < listFileNames.Count; i++)
                                        {
                                                fs.Seek(listFileNames[i], SeekOrigin.Begin);
                                                int bytesToRead;
                                                if (i < listFileNames.Count - 1)
                                                {
                                                        bytesToRead = listFileNames[i + 1] - listFileNames[i];
                                                }
                                                else
                                                {
                                                        bytesToRead = listStartOffsets[0] - listFileNames[i];
                                                }
                                                byte[] fileNameBytes = reader.ReadBytes(bytesToRead);
                                                int endOfNameIndex = Array.FindIndex(fileNameBytes, b => b == 0x00);
                                                if (endOfNameIndex != -1)
                                                {
                                                        fileNameBytes = fileNameBytes.Take(endOfNameIndex).ToArray();
                                                }
                                                string actualFileName = Encoding.ASCII.GetString(fileNameBytes).TrimEnd('\0'); // Remove null terminators

                                                listActualFileNames.Add(actualFileName);
                                        }
                                }
                        }
                }
                private void PopulateDataGrid()
                {
                        var fileData = new List<object>();
                        if (listActualFileNames.Count == listFileLengths.Count && listActualFileNames.Count == listFileNames.Count)
                        {
                                for (int i = 0; i < listActualFileNames.Count; i++)
                                {
                                        fileData.Add(new
                                        {
                                                File = listActualFileNames[i],
                                                Size = listFileLengths[i].ToString(), // File size in bytes
                                                Location = listStartOffsets[i].ToString()
                                        });
                                }
                                fileDataGrid.ItemsSource = null; // Clear the DataGrid first
                                fileDataGrid.ItemsSource = fileData; // Then repopulate it
                        }
                        else
                        {
                                System.Windows.MessageBox.Show(currentLanguageDictionary["Error processing file. Lists are out of sync."], "Error");
                        }
                }
                private void Unpack_Single(string filePath)
                {
                        var selectedItem = fileDataGrid.SelectedItem;
                        if (selectedItem != null)
                        {
                                int selectedIndex = fileDataGrid.SelectedIndex;

                                // Access the Location column (index 2) for the selected row
                                string locationValue = (fileDataGrid.Items[selectedIndex] as dynamic).Location?.ToString();

                                // Check if the Location column contains only numbers or characters (file path)
                                if (!string.IsNullOrEmpty(locationValue) && locationValue.All(char.IsDigit))
                                {
                                        string startOffsetString = (fileDataGrid.Items[selectedIndex] as dynamic).Location?.ToString();
                                        int startOffset = int.TryParse(startOffsetString, out int result) ? result : 0;

                                        // Column 1 (Size) -> fileLength
                                        string fileLengthString = (fileDataGrid.Items[selectedIndex] as dynamic).Size?.ToString();
                                        int fileLength = int.TryParse(fileLengthString, out int lengthResult) ? lengthResult : 0;

                                        // Column 0 (File) -> outputFileName
                                        string outputFileName = (fileDataGrid.Items[selectedIndex] as dynamic).File?.ToString();

                                        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                                        {
                                                fs.Seek(startOffset, SeekOrigin.Begin);
                                                byte[] fileData = new byte[fileLength];
                                                fs.Read(fileData, 0, fileLength);

                                                using (FileStream outputFs = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
                                                {
                                                        outputFs.Write(fileData, 0, fileData.Length);
                                                }
                                        }
                                        PlayWindowsSound("Windows Print complete.wav");
                                        System.Windows.MessageBox.Show($"{outputFileName} {currentLanguageDictionary["File has been successfully unpacked"]}");
                                }
                                else
                                {
                                        //This reult means the above is null empty or contains characters in the location column
                                }
                        }
                        else
                        {
                                PlayWindowsSound("Windows Error.wav");
                                System.Windows.MessageBox.Show(currentLanguageDictionary["No file has been selected."]);
                        }
                }
                private void Unpack_All(string filePath)
                {
                        try
                        {
                                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                                {
                                        using (BinaryReader reader = new BinaryReader(fs))
                                        {
                                                // Loop through all rows in the DataGrid instead of using external lists
                                                for (int i = 0; i < fileDataGrid.Items.Count; i++)
                                                {
                                                        // Access the Location column (index 2) for the current row
                                                        string locationValue = (fileDataGrid.Items[i] as dynamic).Location?.ToString();

                                                        // Check if the Location column contains only digits (i.e., it's a numeric offset)
                                                        if (!string.IsNullOrEmpty(locationValue) && locationValue.All(char.IsDigit))
                                                        {
                                                                // Column 2 (Location) -> startOffset
                                                                int startOffset = int.Parse(locationValue);

                                                                // Column 1 (Size) -> fileLength
                                                                string fileLengthString = (fileDataGrid.Items[i] as dynamic).Size?.ToString();
                                                                int fileLength = int.TryParse(fileLengthString, out int lengthResult) ? lengthResult : 0;

                                                                // Column 0 (File) -> outputFileName
                                                                string outputFileName = (fileDataGrid.Items[i] as dynamic).File?.ToString();

                                                                fs.Seek(startOffset, SeekOrigin.Begin);
                                                                byte[] fileData = new byte[fileLength];
                                                                fs.Read(fileData, 0, fileLength);

                                                                using (FileStream outputFs = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
                                                                {
                                                                        outputFs.Write(fileData, 0, fileData.Length);
                                                                }
                                                        }
                                                        else
                                                        {
                                                                PlayWindowsSound("Windows Error.wav");
                                                                System.Windows.MessageBox.Show(currentLanguageDictionary["Invalid location value at row {i + 1}. Skipping file."]);
                                                                continue;  // Skip the file if the location is invalid
                                                        }
                                                }
                                        }
                                }
                                PlayWindowsSound("Windows Print complete.wav");
                                if (TempVars.DebugMode)
                                {
                                        System.Windows.MessageBox.Show(currentLanguageDictionary["All files in the archive have been successfully unpacked."]);
                                }
                        }
                        catch (Exception ex)
                        {
                                PlayWindowsSound("Windows Error.wav");
                                System.Windows.MessageBox.Show($"Error unpacking files: {ex.Message}");
                        }
                }
                private void ImportFile()
                {
                        //This method checks against importing too large of a file and from adding too many files which would run into the file data offset of 0x1000
                        //Any changes must keep these checks in place
                        OpenFileDialog openFileDialog = new OpenFileDialog { Multiselect = true, Filter = "All Files (*.*)|*.*" };
                        if (openFileDialog.ShowDialog() == true)
                        {
                                string[] selectedFilePaths = openFileDialog.FileNames;
                                var existingFileData = fileDataGrid.ItemsSource != null ? (List<dynamic>)fileDataGrid.ItemsSource : new List<dynamic>();
                                var newFileDataList = new List<dynamic>();

                                // Calculate current metadata length
                                int metadataLength = 0;
                                foreach (dynamic row in fileDataGrid.Items)
                                {
                                        metadataLength += 12 + row.File.Length; // 12 bytes for offsets, plus the length of the filename
                                }

                                // Calculate available free space in metadata area (header + number of files use 20 bytes)
                                int totalMetadataSpace = 0x1000 - 20; // 0x1000 total bytes, 20 used for header and file count
                                int freeSpace = totalMetadataSpace - metadataLength;

                                foreach (string selectedFilePath in selectedFilePaths)
                                {
                                        string fileName = System.IO.Path.GetFileName(selectedFilePath);
                                        int newFileNameLength = 12 + fileName.Length; // New file metadata length (12 bytes dedicated to offsets + filename length)

                                        if (newFileNameLength <= freeSpace)
                                        {
                                                long fileSize = new FileInfo(selectedFilePath).Length;
                                                if (fileSize < 4294967295) // This number comes from us using 4 bytes for the offsets, so we cannot point to a file outside these bounds.
                                                {
                                                        TempVars.NumberofFiles++;
                                                        filePaths.Add(selectedFilePath);
                                                        listActualFileNames.Add(fileName);
                                                        listFileLengths.Add((int)fileSize);

                                                        // Here we add the file and include its full path in the 'Location' column
                                                        newFileDataList.Add(new { File = fileName, Size = fileSize, Location = selectedFilePath });

                                                        freeSpace -= newFileNameLength;
                                                }
                                                else
                                                {
                                                        System.Windows.MessageBox.Show(currentLanguageDictionary["File too large!"] + " > 4GB");
                                                }
                                        }
                                        else
                                        {
                                                // Show warning if not enough space for the new file's metadata
                                                System.Windows.MessageBox.Show(currentLanguageDictionary["Not enough metadata space to add this file."]);
                                        }
                                }

                                // Sort and add new files to the DataGrid
                                var sortedNewFiles = newFileDataList.OrderBy(f => f.File).ToList();
                                existingFileData.AddRange(sortedNewFiles);
                                fileDataGrid.ItemsSource = null; // Clear the existing source to update correctly
                                fileDataGrid.ItemsSource = existingFileData;

                                // Update the label with the new file count
                                if (TempVars.ExistingFile == true)
                                {
                                        TempVars.ChangesHaveBeenMade = true;
                                }

                                label.Content = TempVars.NumberofFiles > 0
                                    ? TempVars.NumberofFiles + currentLanguageDictionary["files found"]
                                    : currentLanguageDictionary["No File Open"];
                        }
                        else
                        {
                                System.Windows.MessageBox.Show(currentLanguageDictionary["No files selected"]);
                        }
                }
                private void RemoveSelectedFile()
                {
                        var selectedItem = fileDataGrid.SelectedItem;
                        if (selectedItem != null)
                        {
                                int selectedIndex = fileDataGrid.SelectedIndex;

                                // Ensure the selected index is valid
                                if (selectedIndex >= 0 && selectedIndex < fileDataGrid.Items.Count)
                                {
                                        // Use pattern matching to get the current items from the DataGrid
                                        if (fileDataGrid.ItemsSource is List<dynamic> currentItems)
                                        {
                                                // Remove the selected item from the list
                                                currentItems.RemoveAt(selectedIndex);

                                                // Also remove the corresponding entries from lists used for file handling
                                                if (selectedIndex < filePaths.Count)
                                                {
                                                        filePaths.RemoveAt(selectedIndex);
                                                        listActualFileNames.RemoveAt(selectedIndex);
                                                }
                                                fileDataGrid.ItemsSource = null; // Clear the DataGrid
                                                fileDataGrid.ItemsSource = currentItems; // Reassign the modified list
                                                TempVars.NumberofFiles--;
                                                label.Content = TempVars.NumberofFiles > 0
                                                    ? TempVars.NumberofFiles + currentLanguageDictionary["files found"]
                                                    : currentLanguageDictionary["No File Open"];
                                                if (TempVars.ExistingFile)
                                                {
                                                        TempVars.ChangesHaveBeenMade = true;
                                                }
                                        }
                                        else
                                        {
                                                System.Windows.MessageBox.Show(currentLanguageDictionary["Error processing file: DataGrid is empty or not properly initialized."]);
                                        }
                                }
                                else
                                {
                                        System.Windows.MessageBox.Show(currentLanguageDictionary["Error processing file. Index is out of range."]);
                                }
                        }
                        else
                        {
                                if (TempVars.DebugMode)
                                {
                                        System.Windows.MessageBox.Show(currentLanguageDictionary["No file has been selected."]);
                                }
                        }
                }
                private void SaveArchive(string filePath)
                {
                        if (fileDataGrid.Items.Count > 0)
                        {
                                //Saving over an archive might be a huge pain.
                                //Method. Extract and write fresh?
                                //Method 2. If files in archive already, skip over but keep moving position, then when you reach a file not in the archive you can enable the writing and it will carry on.
                                //Method 2 not possible dueot offsets at beginning. Would have to move the filenames down.
                                try
                                {
                                        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                                        {
                                                using (BinaryWriter writer = new BinaryWriter(fs))
                                                {
                                                        //Step 1: Write the header (16 bytes)
                                                        byte[] header = new byte[] { 0x61, 0x72, 0x63, 0x68, 0x69, 0x76, 0x65, 0x20, 0x20, 0x56, 0x32, 0x2E, 0x44, 0x4D, 0x5A, 0x00 };
                                                        writer.Write(header);

                                                        //Step 2: Write the number of files (4 bytes, little-endian format)
                                                        int numberOfFiles = fileDataGrid.Items.Count;
                                                        writer.Write(BitConverter.GetBytes(numberOfFiles));

                                                        //Step 3: Calculate and populate filename offsets for all files
                                                        listFileNameOffsetsArchival.Clear();
                                                        int filenameOffset = numberOfFiles * 12 + 20; //20 (16 bytes for header + 4 bytes for filce count)
                                                        listFileNameOffsetsArchival.Add(filenameOffset);

                                                        int nameLengthOffset = 0;
                                                        for (int i = 1; i < numberOfFiles; i++)
                                                        {
                                                                int previousFilenameLength = listActualFileNames[i - 1].Length + 1;
                                                                nameLengthOffset += previousFilenameLength;
                                                                filenameOffset = numberOfFiles * 12 + 20 + nameLengthOffset;
                                                                listFileNameOffsetsArchival.Add(filenameOffset);
                                                        }

                                                        //Step 4: Calculate and populate file data offsets for all files
                                                        listFileDataOffsetsArchival.Clear();
                                                        int fileDataOffset = 0x1000;
                                                        listFileDataOffsetsArchival.Add(fileDataOffset);
                                                        for (int i = 1; i < numberOfFiles; i++)
                                                        {
                                                                int previousFileLength = 0;
                                                                if (i > 0)
                                                                {
                                                                        dynamic previousRow = fileDataGrid.Items[i - 1];
                                                                        if (previousRow != null)
                                                                        {
                                                                                previousFileLength = (int)previousRow.Size + 1;  //Add 1 to include the 00 after each file data
                                                                        }
                                                                }
                                                                fileDataOffset += previousFileLength;
                                                                listFileDataOffsetsArchival.Add(fileDataOffset);
                                                        }

                                                        //Step 5: Write the archive pattern for each file
                                                        for (int i = 0; i < numberOfFiles; i++)
                                                        {
                                                                writer.Write(BitConverter.GetBytes(listFileNameOffsetsArchival[i]));
                                                                writer.Write(BitConverter.GetBytes(listFileDataOffsetsArchival[i]));
                                                                dynamic currentRow = fileDataGrid.Items[i];
                                                                if (currentRow != null)
                                                                {
                                                                        int fileLength = (int)currentRow.Size;
                                                                        writer.Write(BitConverter.GetBytes(fileLength));  //Write the file length (without 00 byte)
                                                                }
                                                        }

                                                        // Step 6: Write the actual filenames with 00 after each filename
                                                        long currentPosition = fs.Position;
                                                        for (int i = 0; i < numberOfFiles; i++)
                                                        {
                                                                byte[] filenameBytes = System.Text.Encoding.ASCII.GetBytes(listActualFileNames[i]);
                                                                writer.Write(filenameBytes);
                                                                writer.Write((byte)0x00);
                                                        }

                                                        // Step 7: Write padding with 00 from current position to file start offset (0x1000)
                                                        long paddingLength = 0x1000 - fs.Position;
                                                        if (paddingLength > 0)
                                                        {
                                                                writer.Write(new byte[paddingLength]);  // Write padding of 00 bytes
                                                        }

                                                        // Step 8: Write the file data with 00 after each file data
                                                        for (int i = 0; i < numberOfFiles; i++)
                                                        {
                                                                string filePathToWrite = filePaths[i];
                                                                byte[] fileData = File.ReadAllBytes(filePathToWrite);
                                                                writer.Write(fileData);  // Write the file data
                                                                writer.Write((byte)0x00); // Write 00 after each file data
                                                        }
                                                        if (TempVars.DebugMode)
                                                        {
                                                                System.Windows.MessageBox.Show(currentLanguageDictionary["Archive saved successfully."]);
                                                        }
                                                        PlayWindowsSound("Windows Print complete.wav");
                                                }
                                        }
                                }
                                catch (Exception ex)
                                {
                                        System.Windows.MessageBox.Show($"Error saving archive: {ex.Message}");
                                }
                        }
                        else
                        {
                                System.Windows.MessageBox.Show(currentLanguageDictionary["There are no files to save."]);
                        }
                }
                private void PlayWindowsSound(string soundFileName)
                {
                        string windowsMediaPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Media", soundFileName);// Path to the built-in Windows sound folder
                        if (System.IO.File.Exists(windowsMediaPath))
                        {
                                SoundPlayer player = new SoundPlayer(windowsMediaPath);
                                player.Play();
                        }
                        else
                        {
                                if (TempVars.DebugMode)
                                {
                                        System.Windows.MessageBox.Show(currentLanguageDictionary["Sound file not found."]);
                                }
                        }
                }
        }
        internal static class TempVars
        {
                internal static bool DebugMode = false;
                internal static bool ExistingFile = false;
                internal static string fileName = "";
                internal static int NumberofFiles = 0;
                internal static bool ChangesHaveBeenMade = false;
        }
        public enum SupportedLanguage
        {
                English,
                Español,
                Français,
                Deutsch,
                Italiano,
                日本語,
                简体中文,
                Русский,
                Українська,
                Polski,
                한국어,
                Português,
                Nederlands,
                Ελληνικά
        }
        public enum SystemTheme
        {
                Light,
                Dark
        }
}