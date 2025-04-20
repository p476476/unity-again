# Again System -- Unity AVG 對話系統

## 目標

- 成為比 Fungus 更好用對話系統
- 精簡化 Fungus，省略一堆用不到的指令和工具
- 對企劃人員友善，支援 Google Sheet 製作對話表，遊戲中可以直接讀取播放
- 對程式人員友善，一個 Prefab 隨放及用，一個一目了然的設定介面

## 使用方法
- [企劃使用說明](./Docs/README_PL.MD)
- [程式使用說明](./Docs/README_RD.MD)

## 需求列表

- 企劃遊戲腳本功能需求
  - [o]顯示人名與文本
  - [o]立繪移動
  - [o]立繪跳一下、放大縮小、移動、震動...等
  - [o]ShakeCamera
  - [o]播放 Spine 動畫
  - [o]切換 Spine 差分
  - [o]等待 Wait (語句講完的 break)
  - [o]進退場的黑幕淡入淡出
  - [o]運鏡 (LookAt 攝影機放大到指定部位-Wait 1.5s-回到原本立繪)
  - [o]切換背景 Background
  - [o]對話框震動
  - [o]文本文字放大縮小
  - [o]立繪明暗切換
- [o]支援 Google Sheet 製作對話表
- [o]支援多語言
- [o]選項選單
- [o]壓住 CTRL 加速對話
- [o]當執行的腳本有錯誤時，能夠在畫面中顯示錯誤
- [x]對話紀錄

## 第三方套件
- [DOTween](https://dotween.demigiant.com/) - 一種簡單且高效的方式，用程式碼來創建和管理物件的動畫的工具 (1.0.380)
- [Spine](https://zh.esotericsoftware.com/spine-unity-download) - 2D 骨骼動畫工具 (4.2.100)