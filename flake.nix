{
  description = "Advent of Code 2023";
  inputs = {
    flake-utils.url = "github:numtide/flake-utils";
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
    nix-ld = {
      url = "github:Mic92/nix-ld";
      inputs.nixpkgs.follows = "nixpkgs";
    };
  };

  outputs = {
    self,
    nixpkgs,
    flake-utils,
    nix-ld,
  }:
    flake-utils.lib.eachDefaultSystem (
      system: let
        pkgs = nixpkgs.legacyPackages.${system};
      in
        # Conditionally include Swift and Apple SDK for Darwin systems
        let
          darwinDeps =
            if system == "x86_64-darwin" || system == "aarch64-darwin"
            then [
              pkgs.swift
              pkgs.darwin.apple_sdk.frameworks.Foundation
              pkgs.darwin.apple_sdk.frameworks.CryptoKit
              pkgs.darwin.apple_sdk.frameworks.GSS
            ]
            else [];
        in let
          deps = darwinDeps ++ [pkgs.zlib pkgs.zlib.dev pkgs.openssl pkgs.icu];
        in {
          devShells = {
            default = pkgs.mkShell {
              HOME = "/tmp/dotnet-home";
              NUGET_PACKAGES = "/tmp/dotnet-home/.nuget/packages";
              LINKER_PATH = "${pkgs.stdenv.cc}/nix-support/dynamic-linker";
              buildInputs = with pkgs;
                [
                  (with dotnetCorePackages;
                    combinePackages [
                      dotnet-sdk_8
                      dotnetPackages.Nuget
                    ])
                ]
                ++ [pkgs.alejandra pkgs.patchelf pkgs.strace];
            };
          };
        }
    );
}
