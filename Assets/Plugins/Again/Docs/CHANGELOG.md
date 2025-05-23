# 版本 0.0.3

## 修正
-   更新字體，解決中文字或全形符號出現亂碼問題

# 版本 0.0.2

## 新功能
-   支援 unity6 版本(6000.0.40f1)
-   新增 Call 指令，可以執行另一個對話腳本
-   新增 PlaySound指令，可以撥放音效
-   新增 HideBackground 指令，可以關閉或淡出背景
-   對話新增auto和skip對話功能
-   新增對話紀錄頁(開發中)
-   新增設定面板，可參考[設定面板說明](./README_RD.MD#設定面板)
-   所有指令新增Join參數，將上Join=True時，可以和上一個指令同時執行
-   新增下載所有腳本到本地端功能，並且可以切換成執行本地端的腳本，可參考[設定面板說明](./README_RD.MD#設定面板)

## 修正
-   修正Mac從Google Sheet抓資料便亂碼問題
-   更新預設字體，解決一些中文字或全形符號出現亂碼問題
-   修正ShowImage指令的淡入淡出無效功能

## 優化
-   移除doozy，都改用原生的UI
-   將可修改的物件做成prefab，改用prefab生成AVG物件，可到設定面板更換自己的prefab，可參考[設定面板說明](./README_RD.MD#設定面板)
-   整理Avg個畫面的顯示層級(Order)，可參考[顯示層級說明](./README_RD.MD#顯示層級)
-   執行腳本時Camera改用overlay的方式加到main camera上，解決需要切換開關攝影機的問題
-   ShowSpine, ScaleSpine, ShowImage, ScaleImage 的新增參數ScaleX 和 ScaleY 可以個別縮放
-   背景圖和圖片統一放在 Resources/Images 底下
-   ChangeBackground 新增 ShowType 參數，當 `ShowType=Fade` 可以做淡入效果
-   ShowImage 新增 ImageName 參數，處理顯示多個相同物件情況，讓 Name 表示物件名； ImageName 表示圖檔名
-   ShowSpine 新增 SpineName 參數，處理顯示多個相同物件情況，讓 Name 表示物件名； SpineName 表示圖檔名

																			