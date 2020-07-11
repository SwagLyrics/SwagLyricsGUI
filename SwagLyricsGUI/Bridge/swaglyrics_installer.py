import sys
import subprocess
import pkg_resources

required = {'swaglyrics'}
installed = {pkg.key for pkg in pkg_resources.working_set}
missing = required - installed

if missing:
    python = sys.executable
    print("Installing swaglyrics...")
    subprocess.check_call([python, '-m', 'pip', 'install', *missing])