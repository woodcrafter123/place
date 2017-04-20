﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Graphics.TileBatch
// Assembly: TerrariaServer, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: 5CBA2320-074B-43F7-8CDC-BF1E2B81EE4B
// Assembly location: C:\Users\kevzhao\Downloads\TerrariaServer.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Terraria.Graphics
{
  public class TileBatch
  {
    private static readonly float[] CORNER_OFFSET_X = new float[4]
    {
      0.0f,
      1f,
      1f,
      0.0f
    };
    private static readonly float[] CORNER_OFFSET_Y = new float[4]
    {
      0.0f,
      0.0f,
      1f,
      1f
    };
    private TileBatch.SpriteData[] _spriteDataQueue = new TileBatch.SpriteData[2048];
    private VertexPositionColorTexture[] _vertices = new VertexPositionColorTexture[8192];
    private GraphicsDevice _graphicsDevice;
    private Texture2D[] _spriteTextures;
    private int _queuedSpriteCount;
    private SpriteBatch _spriteBatch;
    private static Vector2 _vector2Zero;
    private static Rectangle? _nullRectangle;
    private DynamicVertexBuffer _vertexBuffer;
    private DynamicIndexBuffer _indexBuffer;
    private short[] _fallbackIndexData;
    private int _vertexBufferPosition;

    public TileBatch(GraphicsDevice graphicsDevice)
    {
      this._graphicsDevice = graphicsDevice;
      this._spriteBatch = new SpriteBatch(graphicsDevice);
      this.Allocate();
    }

    private void Allocate()
    {
      if (this._vertexBuffer == null || ((GraphicsResource) this._vertexBuffer).get_IsDisposed())
      {
        this._vertexBuffer = new DynamicVertexBuffer(this._graphicsDevice, typeof (VertexPositionColorTexture), 8192, (BufferUsage) 1);
        this._vertexBufferPosition = 0;
        this._vertexBuffer.add_ContentLost((EventHandler<EventArgs>) ((sender, e) => this._vertexBufferPosition = 0));
      }
      if (this._indexBuffer != null && !((GraphicsResource) this._indexBuffer).get_IsDisposed())
        return;
      if (this._fallbackIndexData == null)
      {
        this._fallbackIndexData = new short[12288];
        for (int index = 0; index < 2048; ++index)
        {
          this._fallbackIndexData[index * 6] = (short) (index * 4);
          this._fallbackIndexData[index * 6 + 1] = (short) (index * 4 + 1);
          this._fallbackIndexData[index * 6 + 2] = (short) (index * 4 + 2);
          this._fallbackIndexData[index * 6 + 3] = (short) (index * 4);
          this._fallbackIndexData[index * 6 + 4] = (short) (index * 4 + 2);
          this._fallbackIndexData[index * 6 + 5] = (short) (index * 4 + 3);
        }
      }
      this._indexBuffer = new DynamicIndexBuffer(this._graphicsDevice, typeof (short), 12288, (BufferUsage) 1);
      ((IndexBuffer) this._indexBuffer).SetData<short>((M0[]) this._fallbackIndexData);
      this._indexBuffer.add_ContentLost((EventHandler<EventArgs>) ((sender, e) => ((IndexBuffer) this._indexBuffer).SetData<short>((M0[]) this._fallbackIndexData)));
    }

    private void FlushRenderState()
    {
      this.Allocate();
      this._graphicsDevice.SetVertexBuffer((VertexBuffer) this._vertexBuffer);
      this._graphicsDevice.set_Indices((IndexBuffer) this._indexBuffer);
      this._graphicsDevice.get_SamplerStates().set_Item(0, (SamplerState) SamplerState.PointClamp);
    }

    public void Dispose()
    {
      if (this._vertexBuffer != null)
        ((GraphicsResource) this._vertexBuffer).Dispose();
      if (this._indexBuffer == null)
        return;
      ((GraphicsResource) this._indexBuffer).Dispose();
    }

    public void Begin(Matrix transformation)
    {
      this._spriteBatch.Begin((SpriteSortMode) 0, (BlendState) null, (SamplerState) null, (DepthStencilState) null, (RasterizerState) null, (Effect) null, transformation);
      this._spriteBatch.End();
    }

    public void Begin()
    {
      this._spriteBatch.Begin();
      this._spriteBatch.End();
    }

    public void Draw(Texture2D texture, Vector2 position, VertexColors colors)
    {
      Vector4 destination = (Vector4) null;
      destination.X = position.X;
      destination.Y = position.Y;
      destination.Z = (__Null) 1.0;
      destination.W = (__Null) 1.0;
      this.InternalDraw(texture, ref destination, true, ref TileBatch._nullRectangle, ref colors, ref TileBatch._vector2Zero, (SpriteEffects) 0, 0.0f);
    }

    public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, VertexColors colors, Vector2 origin, float scale, SpriteEffects effects)
    {
      Vector4 destination = (Vector4) null;
      destination.X = position.X;
      destination.Y = position.Y;
      destination.Z = (__Null) (double) scale;
      destination.W = (__Null) (double) scale;
      this.InternalDraw(texture, ref destination, true, ref sourceRectangle, ref colors, ref origin, effects, 0.0f);
    }

    public void Draw(Texture2D texture, Vector4 destination, VertexColors colors)
    {
      this.InternalDraw(texture, ref destination, false, ref TileBatch._nullRectangle, ref colors, ref TileBatch._vector2Zero, (SpriteEffects) 0, 0.0f);
    }

    public void Draw(Texture2D texture, Vector2 position, VertexColors colors, Vector2 scale)
    {
      Vector4 destination = (Vector4) null;
      destination.X = position.X;
      destination.Y = position.Y;
      destination.Z = scale.X;
      destination.W = scale.Y;
      this.InternalDraw(texture, ref destination, true, ref TileBatch._nullRectangle, ref colors, ref TileBatch._vector2Zero, (SpriteEffects) 0, 0.0f);
    }

    public void Draw(Texture2D texture, Vector4 destination, Rectangle? sourceRectangle, VertexColors colors)
    {
      this.InternalDraw(texture, ref destination, false, ref sourceRectangle, ref colors, ref TileBatch._vector2Zero, (SpriteEffects) 0, 0.0f);
    }

    public void Draw(Texture2D texture, Vector4 destination, Rectangle? sourceRectangle, VertexColors colors, Vector2 origin, SpriteEffects effects, float rotation)
    {
      this.InternalDraw(texture, ref destination, false, ref sourceRectangle, ref colors, ref origin, effects, rotation);
    }

    public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, VertexColors colors)
    {
      Vector4 destination = (Vector4) null;
      destination.X = (__Null) (double) (float) destinationRectangle.X;
      destination.Y = (__Null) (double) (float) destinationRectangle.Y;
      destination.Z = (__Null) (double) (float) destinationRectangle.Width;
      destination.W = (__Null) (double) (float) destinationRectangle.Height;
      this.InternalDraw(texture, ref destination, false, ref sourceRectangle, ref colors, ref TileBatch._vector2Zero, (SpriteEffects) 0, 0.0f);
    }

    private static short[] CreateIndexData()
    {
      short[] numArray = new short[12288];
      for (int index = 0; index < 2048; ++index)
      {
        numArray[index * 6] = (short) (index * 4);
        numArray[index * 6 + 1] = (short) (index * 4 + 1);
        numArray[index * 6 + 2] = (short) (index * 4 + 2);
        numArray[index * 6 + 3] = (short) (index * 4);
        numArray[index * 6 + 4] = (short) (index * 4 + 2);
        numArray[index * 6 + 5] = (short) (index * 4 + 3);
      }
      return numArray;
    }

    private unsafe void InternalDraw(Texture2D texture, ref Vector4 destination, bool scaleDestination, ref Rectangle? sourceRectangle, ref VertexColors colors, ref Vector2 origin, SpriteEffects effects, float rotation)
    {
      if (this._queuedSpriteCount >= this._spriteDataQueue.Length)
        Array.Resize<TileBatch.SpriteData>(ref this._spriteDataQueue, this._spriteDataQueue.Length << 1);
      fixed (TileBatch.SpriteData* spriteDataPtr = &this._spriteDataQueue[this._queuedSpriteCount])
      {
        float z = (float) destination.Z;
        float w = (float) destination.W;
        if (sourceRectangle.HasValue)
        {
          Rectangle rectangle = sourceRectangle.Value;
          spriteDataPtr->Source.X = (__Null) (double) (float) rectangle.X;
          spriteDataPtr->Source.Y = (__Null) (double) (float) rectangle.Y;
          spriteDataPtr->Source.Z = (__Null) (double) (float) rectangle.Width;
          spriteDataPtr->Source.W = (__Null) (double) (float) rectangle.Height;
          if (scaleDestination)
          {
            z *= (float) rectangle.Width;
            w *= (float) rectangle.Height;
          }
        }
        else
        {
          float width = (float) texture.get_Width();
          float height = (float) texture.get_Height();
          spriteDataPtr->Source.X = (__Null) 0.0;
          spriteDataPtr->Source.Y = (__Null) 0.0;
          spriteDataPtr->Source.Z = (__Null) (double) width;
          spriteDataPtr->Source.W = (__Null) (double) height;
          if (scaleDestination)
          {
            z *= width;
            w *= height;
          }
        }
        spriteDataPtr->Destination.X = destination.X;
        spriteDataPtr->Destination.Y = destination.Y;
        spriteDataPtr->Destination.Z = (__Null) (double) z;
        spriteDataPtr->Destination.W = (__Null) (double) w;
        spriteDataPtr->Origin.X = origin.X;
        spriteDataPtr->Origin.Y = origin.Y;
        spriteDataPtr->Effects = effects;
        spriteDataPtr->Colors = colors;
        spriteDataPtr->Rotation = rotation;
      }
      if (this._spriteTextures == null || this._spriteTextures.Length != this._spriteDataQueue.Length)
        Array.Resize<Texture2D>(ref this._spriteTextures, this._spriteDataQueue.Length);
      this._spriteTextures[this._queuedSpriteCount++] = texture;
    }

    public void End()
    {
      if (this._queuedSpriteCount == 0)
        return;
      this.FlushRenderState();
      this.Flush();
    }

    private void Flush()
    {
      Texture2D texture = (Texture2D) null;
      int offset = 0;
      for (int index = 0; index < this._queuedSpriteCount; ++index)
      {
        if (this._spriteTextures[index] != texture)
        {
          if (index > offset)
            this.RenderBatch(texture, this._spriteDataQueue, offset, index - offset);
          offset = index;
          texture = this._spriteTextures[index];
        }
      }
      this.RenderBatch(texture, this._spriteDataQueue, offset, this._queuedSpriteCount - offset);
      Array.Clear((Array) this._spriteTextures, 0, this._queuedSpriteCount);
      this._queuedSpriteCount = 0;
    }

    private unsafe void RenderBatch(Texture2D texture, TileBatch.SpriteData[] sprites, int offset, int count)
    {
      this._graphicsDevice.get_Textures().set_Item(0, (Texture) texture);
      float num1 = 1f / (float) texture.get_Width();
      float num2 = 1f / (float) texture.get_Height();
      while (count > 0)
      {
        SetDataOptions setDataOptions = (SetDataOptions) 2;
        int num3 = count;
        if (num3 > 2048 - this._vertexBufferPosition)
        {
          num3 = 2048 - this._vertexBufferPosition;
          if (num3 < 256)
          {
            this._vertexBufferPosition = 0;
            setDataOptions = (SetDataOptions) 1;
            num3 = count;
            if (num3 > 2048)
              num3 = 2048;
          }
        }
        fixed (TileBatch.SpriteData* spriteDataPtr1 = &sprites[offset])
          fixed (VertexPositionColorTexture* positionColorTexturePtr1 = &this._vertices[0])
          {
            TileBatch.SpriteData* spriteDataPtr2 = spriteDataPtr1;
            VertexPositionColorTexture* positionColorTexturePtr2 = positionColorTexturePtr1;
            for (int index1 = 0; index1 < num3; ++index1)
            {
              float num4;
              float num5;
              if ((double) spriteDataPtr2->Rotation != 0.0)
              {
                num4 = (float) Math.Cos((double) spriteDataPtr2->Rotation);
                num5 = (float) Math.Sin((double) spriteDataPtr2->Rotation);
              }
              else
              {
                num4 = 1f;
                num5 = 0.0f;
              }
              float num6 = (float) (spriteDataPtr2->Origin.X / spriteDataPtr2->Source.Z);
              float num7 = (float) (spriteDataPtr2->Origin.Y / spriteDataPtr2->Source.W);
              positionColorTexturePtr2->Color = (__Null) spriteDataPtr2->Colors.TopLeftColor;
              positionColorTexturePtr2[1].Color = (__Null) spriteDataPtr2->Colors.TopRightColor;
              positionColorTexturePtr2[2].Color = (__Null) spriteDataPtr2->Colors.BottomRightColor;
              positionColorTexturePtr2[3].Color = (__Null) spriteDataPtr2->Colors.BottomLeftColor;
              for (int index2 = 0; index2 < 4; ++index2)
              {
                float num8 = TileBatch.CORNER_OFFSET_X[index2];
                float num9 = TileBatch.CORNER_OFFSET_Y[index2];
                float num10 = (float) (((double) num8 - (double) num6) * spriteDataPtr2->Destination.Z);
                float num11 = (float) (((double) num9 - (double) num7) * spriteDataPtr2->Destination.W);
                float num12 = (float) (spriteDataPtr2->Destination.X + (double) num10 * (double) num4 - (double) num11 * (double) num5);
                float num13 = (float) (spriteDataPtr2->Destination.Y + (double) num10 * (double) num5 + (double) num11 * (double) num4);
                if ((spriteDataPtr2->Effects & 2) != null)
                  num8 = 1f - num8;
                if ((spriteDataPtr2->Effects & 1) != null)
                  num9 = 1f - num9;
                // ISSUE: explicit reference operation
                // ISSUE: cast to a reference type
                // ISSUE: explicit reference operation
                (^(Vector3&) @positionColorTexturePtr2->Position).X = (__Null) (double) num12;
                // ISSUE: explicit reference operation
                // ISSUE: cast to a reference type
                // ISSUE: explicit reference operation
                (^(Vector3&) @positionColorTexturePtr2->Position).Y = (__Null) (double) num13;
                // ISSUE: explicit reference operation
                // ISSUE: cast to a reference type
                // ISSUE: explicit reference operation
                (^(Vector3&) @positionColorTexturePtr2->Position).Z = (__Null) 0.0;
                // ISSUE: explicit reference operation
                // ISSUE: cast to a reference type
                // ISSUE: explicit reference operation
                (^(Vector2&) @positionColorTexturePtr2->TextureCoordinate).X = (__Null) ((spriteDataPtr2->Source.X + (double) num8 * spriteDataPtr2->Source.Z) * (double) num1);
                // ISSUE: explicit reference operation
                // ISSUE: cast to a reference type
                // ISSUE: explicit reference operation
                (^(Vector2&) @positionColorTexturePtr2->TextureCoordinate).Y = (__Null) ((spriteDataPtr2->Source.Y + (double) num9 * spriteDataPtr2->Source.W) * (double) num2);
                ++positionColorTexturePtr2;
              }
              ++spriteDataPtr2;
            }
          }
        this._vertexBuffer.SetData<VertexPositionColorTexture>(this._vertexBufferPosition * sizeof (VertexPositionColorTexture) * 4, (M0[]) this._vertices, 0, num3 * 4, sizeof (VertexPositionColorTexture), setDataOptions);
        this._graphicsDevice.DrawIndexedPrimitives((PrimitiveType) 0, 0, this._vertexBufferPosition * 4, num3 * 4, this._vertexBufferPosition * 6, num3 * 2);
        this._vertexBufferPosition += num3;
        offset += num3;
        count -= num3;
      }
    }

    private struct SpriteData
    {
      public Vector4 Source;
      public Vector4 Destination;
      public Vector2 Origin;
      public SpriteEffects Effects;
      public VertexColors Colors;
      public float Rotation;
    }
  }
}
