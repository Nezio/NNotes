   M S B A M L   `   `   `  ����A  =NNotes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:NNotesNNotes  5   	       NNotes.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:NNotes     5   	   -   5          � Title$  NNotes��5   	   $	��330��6   $	��300��6%   ���	#001754F5
���   � � WindowStyle$
 None=�6[    � AllowsTransparency .  � WindowStartupLocation$ CenterScreen=�6�   $	��160��6#  $	��100��62   � Icon$ Resources/icon.ico��6c  .��5	      �  � RowDefinitions 5
   
   �� 5      $��* �6   .+�5   
   �� -   5         ��Name$ richTextBox��5      ���	#01000000$��Segoe UI Light)�6B   $��16(�6^   
����������	#00ABADB3$��-1,25,-1,-3q�6�    �� 
CaretBrush
 ����� �� 
AcceptsTab . 	 �� 
LineHeight$	 1��6�   
 �� SelectionBrush

 �����. �5      /� .T�5      J� 5      5      �� 5   
   -   5         ��Name$ label��5      $��26��6   $��0q�6+   ���	#33000000$	��Top=�6M   .��5      � -   5         �Name$ gridBtn��5      $��Stretch=�6$   $��Stretch=�6B   $	��300��6^   $��20��6j   .+�5      �� -   5         ��Name$ 
imageTheme��5      $
��Left=�6,   $��17��6G   $	��Top=�6S   $��17��6k   $%��Resources/ThemeWhiteAlpha50.png��6v   $��0,-1,0,0q�6�   $y�Hidden=�6�    � ToolTip$ Change theme��66  �� 5      -   5        $ 
imageClose��5      $��Right=�6,   $��13��6H   $��
130,1,12,0q�6T   $	��Top=�6h   $��13��6�   $%��Resources/CloseWhiteAlpha50.png��6�   $ Close��6/  5      5      5      5      