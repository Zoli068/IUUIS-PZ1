   M S B A M L   `   `   `  ����>  :PZ1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:PZ1PZ1  5   	       PZ1.MovieDetailsWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   #localclr-namespace:PZ1     5   	   -   5        $	��600��5   	   $	��700��5	   	     � WindowStyle$
  None=�5
   	    � 
ResizeMode$ NoResize=�5   	    � Title$ MovieDetailsWindow��5   	    � AllowsTransparency . 
������  � WindowStartupLocation$ CenterOwner=�5   	   .��5      ��  �� CornerRadius$ 25��5   	   $��1q�5   	   
���   �
������� � DataContext 5   	   �� *MovieDetail+._�5   
   �  � RowDefinitions 5      �� 5      $��7* �6    �� 5      $��8* �6     � ColumnDefinitions 5       �� 5!      $��* �6#   �� 5"      $��* �6#   .+�5%      �� $ 	25,25,0,0��5&      $��2��5'      ��5)      �� 5*      	 �� ImageSource$ 	 StarWarsSkyBackground2.jpg��6!   ._�5-      �� $��	0,10,0,10q�5.      $��2��5/      $	��650��50      ��51      �� *
ImageUri+53      � 55      $��1��56      $��2��57       59      �� 5:      $��7* �6$   �� 5;      $	��16* �6$   �� 5<      $��3* �6$   �� 5=      $��5* �6$    5@      �� 5A      $��* �6'   �� 5B      $��* �6'   .+�5E      �� $��2��5F      $��0��5G      $��75��5H      $	��700��5I      $��0,1,0,1q�5J      
�������$	��Top=�5L      $��Center=�5M      $��Center=�5N      .��5P      �� $	��660��5Q      
 �� TextAlignment$
 Center=�5R      $
��Wrap=�5S      $��30(�5T      ��5U      �� *Title+5W      �� 5Y      -   5         ��Name$ RichTextBoxDescription��5Z      $��1��5[      $��2��5\       J� 	AllowDrop .   �� 
IsReadOnly . $��5q�5_      
���   �$��0q�5a       �� SelectionBrush
 ���� 5b      �� 5d      $��2��5e      $��2��5f      
�������$��0,1,0,1q�5h      ._�5j      � .+�5l      ��  � ToolTip$  Rating of the movie (0-10)��5m      $��13,0,0,0q�5n      $��Center=�5o       �� Orientation$ 
Horizontal=�5p      .+�5r      �� $��20(�6%   $��Rating:��63   �� 5s      $��20(�6%   ��63   �� *Rating+5u      �� 5w      $��1��5x      $��2��5y      $��0,0,13,0q�5z      $" Movie got added on this date��5{      $��Center=�5|      $��Right=�5}      $ 
Horizontal=�5~      .+�5�      �� $��20(�6$   $��Date:��62   �� 5�      $��20(�6$   ��62   �� *	DateAdded+5�      5�      5�      �� 5�      -   5         ��Name$ 
ExitButton��5�      $��3��5�      $��2��5�      $��40��5�      $	��130��5�      $��34(�5�      $
��Exit��5�      $ Close the detailed view��5�      $��Center=�5�      5�      5�      5�      5�      