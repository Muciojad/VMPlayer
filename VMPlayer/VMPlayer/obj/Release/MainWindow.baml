   M S B A M L   `   `   `  ���� C  ?VMPlayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:VMPlayerVMPlayer      VMPlayer.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     (localclr-namespace:VMPlayer       � Title$  VMPlayer��$	��465��$	��300�����	#FF252525 � 
ResizeMode$ NoResize=�.���  J� OpacityMask
 ��������	#FF252525 � ColumnDefinitions �� $	��45* ��� $
��247* �.+��� -    ��Name$ PlayBtn��$�� ��$
��Left=�$��
75,337,0,0q�$	��Top=�$��40��$��1����Z� ����  �� ImageSource$ images/play_pause-512.png�� j� Stretch$ UniformToFill=��� -   $ NextBtn��$�� ��$
��Left=�$��120,337,0,0q�$	��Top=�$��40��$��1����Z� ���� $+ %images/fast-forward-arrows-symbol.png��$ Uniform=��� -   $ PrevBtn��$�� ��$
��Left=�$��
30,337,0,0q�$	��Top=�$��40�� J� RenderTransformOrigin$ -0.803,0.75&�$��1����Z� ���� $, &images/fast-backward-arrows-button.png��$ Uniform=����� $, &images/fast-backward-arrows-button.png��$ Uniform=��� -    ��Name$ Repeat��$�� ��$
��Left=�$��220,340,0,0q�$	��Top=�$��15��$ 1.31,-0.932&�$��15��$��1���� -   $ Shuffle��$�� ��$��
10,340,0,0q�$��15��$ 1.31,-0.932&�$��15��$	��Top=�$
��Left=���Z� ��Z� �� -   $ OpenBtn��$�� ��$
��Left=�$��
30,396,0,0q�$	��Top=�$��20��#��� ��$��2����Z� ���� $! images/magnifying-glass.png��$ Uniform=��� -   	 ��Name$	 Title��$��Title��$
��Left=�$��
20,172,0,0q�$	��Top=�$	��250��$��2��$
��Bold"�$��15(����	#FFBBBBBB�� -   $	 Album��$��Album��$
��Left=�$��
20,224,0,0q�$	��Top=�$	��250��$��2��$
��Bold"����	#FFBBBBBB�� -	   $ 
DeleteSong��$�� ��$
��Left=�$��205,396,0,0q�$	��Top=�$��20��$ 3.7,0.6&�$��1����Z� ���� $* $images/rubbish-bin-delete-button.png��$ Uniform=��� -
   $	 SongDuration��$��00:00��$
��Left=�$��190,268,0,0q�$	��Top=�$��45��$��Black"����	#FFBBBBBB$��1��� -   
 �Name$
 SongElapsedTime��$
��Left=�$��17��$��
10,272,0,0q�$	��Top=�$	��180�� � Value$ 0Q� � ForceCursor . ���	#00FFFFFF���	#FF817F7F$��1����Z� �� -   $	 SongElapsedLabel��$��00:00��$
��Left=�$��
10,268,0,0q�$	��Top=�$��45��$��Black"����	#FFBBBBBB$��2���� -    ��Name$ VolumeSlider��$
��Left=�$��
82,329,0,0q�$	��Top=�$��22�� �� Orientation$ Vertical=�$��99��$	 100Q�$ 0.5,0.5&�$��1����Z� z�V� .~��� �� ��  �� Angle$ 90Q�T� �� -   $	 Artist��$��Artist��$
��Left=�$��
20,198,0,0q�$	��Top=�$	��250��$��2��$
��Bold"����	#FFBBBBBB�� $
��Left=�$	��150��$	��Top=�$	��250��$��	20,20,0,0q�$��2������ $ images/Hroef.jpg���� $
��Left=�$��20��$	��Top=�$��40��$��
20,337,0,0q�$��2������ $1 +images/couple-of-arrows-changing-places.png��$ Uniform=��� $
��Left=�$��20��$	��Top=�$��40��$��185,337,0,0q�$��1������ $ images/repeat.png��$ Uniform=�