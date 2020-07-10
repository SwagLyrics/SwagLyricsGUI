from swaglyrics import cli
from SwSpotify import spotify
from time import sleep
import sys
import base64

def print_lyrics(song, artist):
    print(base64.b64encode(f"{song} by {artist}:".encode("utf-8")), flush=True)
    print(base64.b64encode(cli.get_lyrics(song, artist).encode('utf-8')), flush=True)

last_song, last_artist = spotify.current()
print_lyrics(last_song, last_artist)

while True:
    new_song, new_artist = spotify.current()
    if new_song != last_song or new_artist != last_artist:
        print_lyrics(new_song, new_artist)
        last_song = new_song
        last_artist = new_artist
    sleep(2)