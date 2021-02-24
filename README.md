# SharpShell

## Description(これ何)  
これはCSharpで書かれたシェルソフトウェアです.  
Zshやbashのような何かです.  

## Constitution(ファイル構成)  
- lib
コマンド入力により呼び出される奴ら
- mod
入出力, 入力解析, コマンド割り当て(まだない)を担当奴ら
- misc
その他
- process
プロセス管理系の奴ら, 使うかは知らん.
- rule
型定義が入ってる
- log(まだない)
ログ残すやつ

## Usage(使い方)  
このソフトウェア単体で使用されることは想定していません.  
[Sharminal](https://github.com/x0y14/sharminal) (まだ404)に組み込まれる予定です.    
(現在Sharminalはプライベートリポジトリで開発されています.)  

## Attention(注意)  
このソフトウェアは性質上, コンピュータ上で多くのデータを参照します.  
そのため, 重要なデータが存在しているコンピュータ等での使用はお控えください.  
そして, 深刻なバグが発見された場合でも, すぐにアップデートがされない場合があります.  
このソフトウェアを使用したことによる障害, データ破損, 損害について責任を負うことはできません.  
くれぐれも自己責任で利用してください.  

## How it works(どのように動くのか)  
`破壊的変更が入る可能性が高いことがあります. `  
`また, この情報は古い可能性があります.`  
1. SharpShellが起動される
	1. UserName, WhereAmIが実行され初期値が代入される.
	2. その他の初期化.
	3. Init関数でユーザー設定を読み込む.
		1. "welcome-message"が設定されていた場合, このタイミングで表示される.
	4. listener(ユーザー入力監視), assignor(コマンド検索, 割り当て)を初期化.
	5. SharpShell.LineStreaming関数を起動,
		1. この下を終了入力があるまでループ
			1. listener.Listening関数にプロンプトスタイルを入れ, 入力待ち.
			2. 受け取った入力をassignor.Assaign関数へ入れ, コマンドなのかそうでないのかを判定.
				- ビルトイン関数の指令であれば, BuiltinManagerへコマンド内容を渡し, 戻り値をコンソールへ出力.
				- その他の関数であれば, ProcessManagerへコマンド内容を渡し, 戻り値コンソールへ出力.
				- その他の無効な入力だった場合はエラーを出力.

#### Memo
new shell()  
shell.init()  
shell(#input) -> listener -> parser -> listener -> shell  
shell -> assignor -> shell  
shell -> processManager -> shell(#output)  