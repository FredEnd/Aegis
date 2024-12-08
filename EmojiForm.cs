using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aegis
{
    public partial class EmojiForm : Form
    {
        private TextBox Input_Box;
        private FlowLayoutPanel Emoji_Panel;
        private Button PreviousButton, NextButton;

        private const int EmojisPerPage = 100; // Number of emojis per page
        private int currentPage = 0; // Current page index
        private string[] allEmojis; // Array to hold all emojis

        public EmojiForm(TextBox Input_Box)
        {
            this.Input_Box = Input_Box;

            InitializeComponent();
            InitializeUI();
            LoadEmojis();
            ShowEmojisForPage();
        }

        private void InitializeUI()
        {

            this.Text = "Emoji Panel";
            this.Size = new Size(400, 800);

            Emoji_Panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true
            };
            this.Controls.Add(Emoji_Panel);

            Panel navigationPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50
            };
            this.Controls.Add(navigationPanel);

            PreviousButton = new Button
            {
                Text = "◀️ Previous",
                Width = 100,
                Height = 40,
                Enabled = false
            };
            PreviousButton.Click += PreviousButton_Click;
            navigationPanel.Controls.Add(PreviousButton);

            NextButton = new Button
            {
                Text = "Next ▶️",
                Width = 100,
                Height = 40,
                Left = 110
            };
            NextButton.Click += NextButton_Click;
            navigationPanel.Controls.Add(NextButton);
        }

        private void LoadEmojis()
        {
            this.allEmojis = new string[]
                    {
            // Smileys & Emotion
            "😀", "😃", "😄", "😁", "😆", "😅", "😂", "🤣", "😊", "😇", "🙂", "🙃", "😉", "😌", "😍", "😘", "😗", "😙", "😚",
            "😋", "😛", "😜", "🤪", "😝", "🤑", "🤗", "🤭", "🤫", "🤔", "🤐", "🤨", "😐", "😑", "😶", "😏", "😒", "🙄", "😬",
            "😮", "😯", "😲", "🥺", "😦", "😧", "😨", "😰", "😥", "😢", "😭", "😱", "😖", "😣", "😞", "😓", "😩", "😫", "🥱",
            "😤", "😡", "😠", "🤬", "😈", "👿", "💀", "☠️", "💩", "🤡", "👹", "👺", "👻", "👽", "👾", "🤖", "🎃", "😺", "😸",
            "😹", "😻", "😼", "😽", "🙀", "😿", "😾",

            // People & Body
            "👶", "👧", "👦", "👩", "👨", "👩‍🦱", "👨‍🦱", "👩‍🦳", "👨‍🦳", "👩‍🦲", "👨‍🦲", "👱‍♀️", "👱‍♂️", "👴", "👵", "🙍‍♂️",
            "🙍‍♀️", "🙎‍♂️", "🙎‍♀️", "🙅‍♂️", "🙅‍♀️", "🙆‍♂️", "🙆‍♀️", "💁‍♂️", "💁‍♀️", "🙋‍♂️", "🙋‍♀️", "🧏‍♂️", "🧏‍♀️", "🙇‍♂️",
            "🙇‍♀️", "🤦‍♂️", "🤦‍♀️", "🤷‍♂️", "🤷‍♀️", "👨‍⚕️", "👩‍⚕️", "👨‍🎓", "👩‍🎓", "👨‍🏫", "👩‍🏫", "👨‍⚖️", "👩‍⚖️", "👨‍🌾",
            "👩‍🌾", "👨‍🍳", "👩‍🍳", "👨‍🔧", "👩‍🔧", "👨‍🏭", "👩‍🏭", "👨‍💼", "👩‍💼", "👨‍🔬", "👩‍🔬", "👨‍💻", "👩‍💻", "👨‍🎤", "👩‍🎤",

            // Animals & Nature
            "🐶", "🐱", "🐭", "🐹", "🐰", "🦊", "🐻", "🐼", "🐨", "🐯", "🦁", "🐮", "🐷", "🐽", "🐸", "🐵", "🙈", "🙉", "🙊",
            "🐒", "🐔", "🐧", "🐦", "🐤", "🐣", "🐥", "🦆", "🦅", "🦉", "🦇", "🐺", "🐗", "🐴", "🦄", "🐝", "🐛", "🦋", "🐌",
            "🐞", "🐜", "🦟", "🦗", "🕷️", "🕸️", "🐢", "🐍", "🦎", "🦂", "🦞", "🦀", "🦐", "🐠", "🐡", "🐬", "🐳", "🐋", "🦈",

            // Food & Drink
            "🍇", "🍈", "🍉", "🍊", "🍋", "🍌", "🍍", "🥭", "🍎", "🍏", "🍐", "🍑", "🍒", "🍓", "🥝", "🍅", "🥥", "🥑", "🍆",
            "🥔", "🥕", "🌽", "🌶️", "🥒", "🥬", "🥦", "🧄", "🧅", "🍄", "🥜", "🌰", "🍞", "🥐", "🥖", "🥨", "🥯", "🥞", "🧇",
            "🧀", "🍗", "🍖", "🥩", "🍤", "🍳", "🥘", "🍲", "🥣", "🥗", "🍿", "🧈", "🍱", "🍚", "🍛", "🍜", "🍝", "🍠", "🍢",
            "🍣", "🍤", "🍥", "🥮", "🍡", "🥟", "🥠", "🥡", "🦪", "🍦", "🍧", "🍨", "🍩", "🍪", "🎂", "🍰", "🧁", "🥧", "🍫",

            // Travel & Places
            "🚗", "🚕", "🚙", "🚌", "🚎", "🏎️", "🚓", "🚑", "🚒", "🚐", "🚚", "🚛", "🚜", "🏍️", "🚲", "🛴", "🚨", "🚔", "🚍",
            "🛺", "🚝", "🚅", "🚈", "🚞", "🚋", "🚃", "🚂", "🛩️", "✈️", "🛫", "🛬", "🚁", "🚀", "🛸", "🚤", "🛳️", "⛴️", "⚓",
            "🚧", "🗿", "🗽", "🗼", "🏰", "🏯", "🏟️", "🎡", "🎢", "🎠", "🏖️", "🏜️", "🏝️", "🏞️", "🏔️", "🗻", "🌋", "🗾", "🏠",

            // Objects & Symbols
            "⌚", "📱", "📲", "💻", "🖥️", "🖨️", "⌨️", "🖱️", "🖲️", "💾", "💿", "📀", "📼", "📷", "📸", "📹", "🎥", "📽️", "🎞️",

            // Flags
            "🇦🇫", "🇦🇱", "🇩🇿", "🇦🇸", "🇦🇩", "🇦🇴", "🇦🇮", "🇦🇶", "🇦🇬", "🇦🇷", "🇦🇲", "🇦🇼", "🇦🇺", "🇦🇹", "🇦🇿", "🇧🇸",
            "🇧🇭", "🇧🇩", "🇧🇧", "🇧🇾", "🇧🇪", "🇧🇿", "🇧🇯", "🇧🇲", "🇧🇹", "🇧🇴", "🇧🇦", "🇧🇼", "🇧🇷", "🇧🇳", "🇧🇬", "🇧🇫",
            "🇧🇮", "🇰🇭", "🇨🇲", "🇨🇦", "🇨🇻", "🇧🇶", "🇨🇫", "🇨🇱", "🇨🇳", "🇨🇴", "🇨🇬", "🇨🇩", "🇨🇷", "🇨🇺", "🇨🇾", "🇨🇿",
            "🇩🇰", "🇩🇯", "🇩🇲", "🇩🇴", "🇪🇨", "🇪🇬", "🇸🇻", "🇬🇶", "🇪🇷", "🇪🇪", "🇪🇹", "🇫🇯", "🇫🇮", "🇫🇷", "🇬🇦", "🇬🇲",
            "🇬🇪", "🇩🇪", "🇬🇭", "🇬🇷", "🇬🇩", "🇬🇺", "🇬🇹", "🇬🇳", "🇬🇼", "🇬🇾", "🇭🇹", "🇭🇳", "🇭🇰", "🇭🇺", "🇮🇸", "🇮🇳",
            "🇮🇩", "🇮🇷", "🇮🇶", "🇮🇪", "🇮🇱", "🇮🇹", "🇯🇲", "🇯🇵", "🇯🇴", "🇰🇿", "🇰🇪", "🇰🇮", "🇰🇵", "🇰🇷", "🇽🇰", "🇰🇼",
            "🇰🇬", "🇱🇦", "🇱🇻", "🇱🇧", "🇱🇸", "🇱🇷", "🇱🇾", "🇱🇮", "🇱🇹", "🇱🇺", "🇲🇴", "🇲🇰", "🇲🇬", "🇲🇼", "🇲🇾", "🇲🇻",
            "🇲🇱", "🇲🇹", "🇲🇭", "🇲🇶", "🇲🇷", "🇲🇺", "🇲🇽", "🇫🇲", "🇲🇩", "🇲🇨", "🇲🇳", "🇲🇪", "🇲🇸", "🇲🇦", "🇲🇿", "🇲🇲",
            "🇳🇦", "🇳🇷", "🇳🇵", "🇳🇱", "🇳🇨", "🇳🇿", "🇳🇮", "🇳🇪", "🇳🇬", "🇳🇺", "🇳🇫", "🇲🇵", "🇳🇴", "🇴🇲", "🇵🇰", "🇵🇼",
            "🇵🇸", "🇵🇦", "🇵🇬", "🇵🇾", "🇵🇪", "🇵🇭", "🇵🇱", "🇵🇹", "🇵🇷", "🇶🇦", "🇷🇴", "🇷🇺", "🇷🇼", "🇼🇸", "🇸🇲", "🇸🇦",
            "🇸🇳", "🇷🇸", "🇸🇨", "🇸🇱", "🇸🇬", "🇸🇰", "🇸🇮", "🇸🇧", "🇸🇴", "🇿🇦", "🇪🇸", "🇱🇰", "🇸🇩", "🇸🇷", "🇸🇪", "🇨🇭",
            "🇸🇾", "🇹🇼", "🇹🇯", "🇹🇿", "🇹🇭", "🇹🇱", "🇹🇬", "🇹🇰", "🇹🇴", "🇹🇹", "🇹🇳", "🇹🇷", "🇹🇲", "🇹🇻", "🇺🇬", "🇺🇦",
            "🇦🇪", "🇬🇧", "🇺🇸", "🇺🇾", "🇺🇿", "🇻🇺", "🇻🇦", "🇻🇪", "🇻🇳", "🇾🇪", "🇿🇲", "🇿🇼"
            };
        }

        private void ShowEmojisForPage()
        {

            Emoji_Panel.Controls.Clear();

            int start = currentPage * EmojisPerPage;
            int end = Math.Min(start + EmojisPerPage, allEmojis.Length);

            for (int i = start; i < end; i++)
            {
                Button emojiButton = new Button
                {
                    Text = allEmojis[i],
                    Font = new Font("Segoe UI Emoji", 12),
                    Size = new Size(50, 50),
                    Margin = new Padding(5)
                };
                emojiButton.Click += EmojiButton_Click;
                Emoji_Panel.Controls.Add(emojiButton);
            }

            UpdateNavigationButtons();
        }

        private void UpdateNavigationButtons()
        {
            PreviousButton.Enabled = currentPage > 0;
            NextButton.Enabled = (currentPage + 1) * EmojisPerPage < allEmojis.Length;
        }

        private void PreviousButton_Click(object sender, EventArgs e)
        {
            if (currentPage > 0)
            {
                currentPage--;
                ShowEmojisForPage();
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if ((currentPage + 1) * EmojisPerPage < allEmojis.Length)
            {
                currentPage++;
                ShowEmojisForPage();
            }
        }

        private void EmojiButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                this.Input_Box.Text += button.Text;
                this.Close();
            }
        }
    }
}
