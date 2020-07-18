import time
import sys
import base64
import os
import tempfile

from swaglyrics import cli
from SwSpotify import spotify, SpotifyNotRunning

paused = False

def print_encoded(data):
    print(base64.b64encode(data.encode('utf-8')), flush=True)

def print_lyrics(song, artist):
    print_encoded(f"{song} by {artist}:")
    lyrics = cli.get_lyrics(song, artist)
    if lyrics is None:
        lyrics = "Couldn't get lyrics"
    print_encoded(lyrics)
    sys.stdout.flush()

last_song = ""
last_artist = ""
new_song = ""
new_artist = ""

while True:
    try:
        new_song, new_artist = spotify.current()
    except SpotifyNotRunning:
        print("SpotifyNotRunning", file=sys.stderr, flush=True)
        paused = True
        sys.stderr.flush()
    else:
        if new_song != last_song or new_artist != last_artist:
            print_lyrics(new_song, new_artist)
            last_song = new_song
            last_artist = new_artist
            paused = False
        elif paused:
            print_encoded("Resumed")
    if not os.path.exists(os.path.join(tempfile.gettempdir(), "SwagLyricsGUI", "swaglyricsGUIOn.txt")): # kills the process if GUI is off and not closed it properly
        sys.exit()
    time.sleep(2)